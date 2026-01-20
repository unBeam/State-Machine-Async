using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Application.StateMachine;
using Game.Core.Scripts.Domain.StateMachine;

namespace Game.Features.Enemies.Application
{
    public sealed class EnemyBrain : IDisposable
    {
        private readonly StateMachine<EnemyStateID> _stateMachine;
        private readonly EnemyStateID _initialStateId;

        public EnemyBrain(Dictionary<EnemyStateID, IState> states, EnemyStateID initialStateId)
        {
            _stateMachine = new StateMachine<EnemyStateID>(states);
            _initialStateId = initialStateId;
        }

        public EnemyStateID CurrentStateId => _stateMachine.CurrentStateId;

        public bool IsDead => _stateMachine.HasState && _stateMachine.CurrentStateId == EnemyStateID.Dead;

        public void Tick(float deltaTime)
        {
            _stateMachine.Tick(deltaTime);
        }

        public UniTask StartAsync(CancellationToken cancellationToken)
        {
            return _stateMachine.ChangeStateAsync(_initialStateId, cancellationToken);
        }

        public UniTask SetStateAsync(EnemyStateID stateId, CancellationToken cancellationToken)
        {
            if (IsDead && stateId != EnemyStateID.Dead)
            {
                return UniTask.CompletedTask;
            }

            return _stateMachine.ChangeStateAsync(stateId, cancellationToken);
        }

        public UniTask KillAsync(CancellationToken cancellationToken)
        {
            return _stateMachine.ChangeStateAsync(EnemyStateID.Dead, cancellationToken);
        }

        public void Dispose()
        {
            _stateMachine.Dispose();
        }
    }
}