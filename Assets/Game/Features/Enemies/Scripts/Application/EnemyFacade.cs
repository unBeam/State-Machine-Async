using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Application;
using Game.Features.Enemies.Application.States;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Presentation.Unity;
using Game.Features.Enemies.Presentation.Unity.Configs;
using Game.Features.Enemies.Presentation.Unity.Factories;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Features.Enemies.Unity
{
    public sealed class EnemyFacade : MonoBehaviour, IDisposable
    {
        [SerializeField] private EnemyDefinition _definition;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _muzzle;

        private EnemyBrain _brain;
        private EnemyComposite _composite;
        private CancellationTokenSource _cts;

        [Inject] private DiContainer _container;
        [Inject] private EnemyCompositeFactory _compositeFactory;

        private void Awake()
        {
            if (_definition == null)
            {
                throw new InvalidOperationException("EnemyDefinition is not set.");
            }

            EnemyContext context = new EnemyContext(transform, _target);
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            _composite = _compositeFactory.Create(_definition, context, agent, _muzzle);

            Dictionary<EnemyStateID, IState> states = CreateStates(_definition.BrainConfig, _composite);

            _brain = new EnemyBrain(states, _definition.BrainConfig.InitialStateId);
            _cts = new CancellationTokenSource();
        }

        private void Start()
        {
            _brain.StartAsync(_cts.Token).Forget();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _brain.Tick(dt);
        }

        private Dictionary<EnemyStateID, IState> CreateStates(EnemyBrainConfig brainConfig, EnemyComposite composite)
        {
            Dictionary<EnemyStateID, IState> states = new Dictionary<EnemyStateID, IState>(brainConfig.RequiredStates.Length);

            EnemyStateID[] required = brainConfig.RequiredStates;

            for (int i = 0; i < required.Length; i++)
            {
                EnemyStateID id = required[i];
                states[id] = CreateState(id, composite);
            }

            return states;
        }

        private IState CreateState(EnemyStateID id, EnemyComposite composite)
        {
            if (id == EnemyStateID.Idle)
            {
                return _container.Instantiate<EnemyIdleState>();
            }

            if (id == EnemyStateID.Engage)
            {
                IEnemyStateSwitcher switcher = new EnemyStateSwitcher(() => _brain);
                return new EnemyEngageState(composite, switcher);
            }

            if (id == EnemyStateID.Attack)
            {
                IEnemyStateSwitcher switcher = new EnemyStateSwitcher(() => _brain);
                return new EnemyAttackState(composite, switcher);
            }

            if (id == EnemyStateID.Dead)
            {
                return _container.Instantiate<EnemyDeadState>();
            }

            throw new InvalidOperationException("Unknown state id: " + id);
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            if (_brain != null)
            {
                _brain.Dispose();
                _brain = null;
            }

            if (_composite != null)
            {
                _composite.Dispose();
                _composite = null;
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
