using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Domain.StateMachine;

namespace Game.Core.Scripts.Application.StateMachine
{
    public sealed class StateMachine<TStateId> : IDisposable where TStateId : struct, Enum
    {
        private readonly Dictionary<TStateId, IState> _states;
        private readonly object _gate;
        private CancellationTokenSource _stateCts;
        private IState _currentState;
        private TStateId _currentStateId;
        private bool _hasState;
        private int _transitionVersion;
        
        public bool HasState => _hasState;  
        public TStateId CurrentStateId => _currentStateId;
        
        public StateMachine(Dictionary<TStateId, IState> states)
        {
            _states = states;
            _gate = new object();
        }
        
        public void Tick(float deltaTime)
        {
            IState state = _currentState;
            state?.Tick(deltaTime);
        }

        public UniTask ChangeStateAsync(TStateId id, CancellationToken cancellationToken)
        {
            return ChangeStateInternalAsync(id, cancellationToken);
        }

        private async UniTask ChangeStateInternalAsync(TStateId id, CancellationToken cancellationToken)
        {
            IState nextState;
            if (!_states.TryGetValue(id, out nextState))
            {
                throw new InvalidOperationException($"State not registered - {id}");
            }

            IState previousState;
            CancellationTokenSource previousCts;
            CancellationTokenSource nextCts;
            int myVersion;

            lock (_gate)
            {
                _transitionVersion++;
                myVersion = _transitionVersion;

                previousState = _currentState;
                previousCts = _stateCts;

                nextCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                _stateCts = nextCts;

                _currentState = nextState;
                _currentStateId = id;
                _hasState = true;
            }

            CancelAndDispose(previousCts);

            if (previousState != null)
            {
                await previousState.ExitAsync(CancellationToken.None);
            }

            if (!IsCurrentTransition(myVersion, nextCts))
            {
                CancelAndDispose(nextCts);
                return;
            }

            await nextState.EnterAsync(nextCts.Token);
        }

        public void Dispose()
        {
            CancellationTokenSource cts;

            lock (_gate)
            {
                cts = _stateCts;
                _stateCts = null;
                _currentState = null;
                _hasState = false;
                _transitionVersion++;
            }

            CancelAndDispose(cts);
        }

        private bool IsCurrentTransition(int version, CancellationTokenSource cts)
        {
            lock (_gate)
            {
                if (!_hasState)
                {
                    return false;
                }

                if (version != _transitionVersion)
                {
                    return false;
                }

                return ReferenceEquals(_stateCts, cts);
            }
        }

        private static void CancelAndDispose(CancellationTokenSource cts)
        {
            if (cts == null)
            {
                return;
            }

            cts.Cancel();
            cts.Dispose();
        }
    }
}
