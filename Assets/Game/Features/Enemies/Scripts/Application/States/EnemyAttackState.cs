using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Domain;

namespace Game.Features.Enemies.Application.States
{
    public sealed class EnemyAttackState : IState
    {
        private readonly EnemyComposite _composite;
        private readonly IEnemyStateSwitcher _switcher;
        private int _isAttacking;

        public EnemyAttackState(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            _composite = composite;
            _switcher = switcher;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            _isAttacking = 0;
            _composite.Movement.Stop();
            return _composite.Attack.EnterAsync(cancellationToken);
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            _isAttacking = 0;
            return _composite.Attack.ExitAsync(cancellationToken);
        }

        public void Tick(float deltaTime)
        {
            _composite.Attack.Tick(deltaTime);

            if (_composite.Movement.IsInAttackRange() == false)
            {
                _switcher.SetStateAsync(EnemyStateID.Engage, CancellationToken.None).Forget();
                return;
            }

            if (_isAttacking == 1)
            {
                return;
            }

            if (_composite.Attack.CanAttack() == false)
            {
                return;
            }

            _isAttacking = 1;
            AttackRoutine().Forget();
        }

        private async UniTaskVoid AttackRoutine()
        {
            try
            {
                await _composite.Attack.AttackAsync(CancellationToken.None);
            }
            finally
            {
                _isAttacking = 0;
            }
        }
    }
}