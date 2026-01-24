using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Domain;

namespace Game.Features.Enemies.Application.States
{
    public sealed class EnemyEngageState : IState
    {
        private readonly EnemyComposite _composite;
        private readonly IEnemyStateSwitcher _switcher;

        private CancellationToken _lifetimeToken;

        public EnemyEngageState(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            _composite = composite;
            _switcher = switcher;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            _lifetimeToken = cancellationToken;
            return _composite.Movement.EnterAsync(cancellationToken);
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            return _composite.Movement.ExitAsync(cancellationToken);
        }

        public void Tick(float deltaTime)
        {
            _composite.Movement.Tick(deltaTime);

            if (_composite.Movement.IsInAttackRange())
            {
                _switcher.SetStateAsync(EnemyStateID.Attack, _lifetimeToken).Forget();
            }
        }
    }
}