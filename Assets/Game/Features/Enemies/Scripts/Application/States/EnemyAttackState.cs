using System;
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

        private bool _isAttacking;
        private CancellationToken _lifetimeToken;
        private CancellationTokenSource _attackCts;

        public EnemyAttackState(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            _composite = composite;
            _switcher = switcher;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            _lifetimeToken = cancellationToken;
            _isAttacking = false;

            DisposeAttackCts();
            _attackCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeToken);

            _composite.Movement.Stop();
            return _composite.Attack.EnterAsync(cancellationToken);
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            _isAttacking = false;
            CancelAttackCts();
            return _composite.Attack.ExitAsync(cancellationToken);
        }

        public void Tick(float deltaTime)
        {
            _composite.Attack.Tick(deltaTime);

            if (_composite.Movement.IsInAttackRange() == false)
            {
                _switcher.SetStateAsync(EnemyStateID.Engage, _lifetimeToken).Forget();
                return;
            }

            if (_isAttacking)
            {
                return;
            }

            if (_composite.Attack.CanAttack() == false)
            {
                return;
            }

            _isAttacking = true;
            AttackRoutine().Forget();
        }

        private async UniTaskVoid AttackRoutine()
        {
            CancellationToken token = _attackCts != null ? _attackCts.Token : _lifetimeToken;

            try
            {
                await _composite.Attack.AttackAsync(token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _isAttacking = false;
            }
        }

        private void CancelAttackCts()
        {
            if (_attackCts == null)
            {
                return;
            }

            _attackCts.Cancel();
            DisposeAttackCts();
        }

        private void DisposeAttackCts()
        {
            if (_attackCts == null)
            {
                return;
            }

            _attackCts.Dispose();
            _attackCts = null;
        }
    }
}
