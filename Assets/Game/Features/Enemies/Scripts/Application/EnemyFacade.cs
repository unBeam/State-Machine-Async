using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Combat.Domain;
using Game.Features.Enemies.Application;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Presentation.Unity;
using Game.Features.Enemies.Presentation.Unity.Configs;
using Game.Features.Enemies.Presentation.Unity.Factories;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Features.Enemies.Unity
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyTeam))]
    public sealed class EnemyFacade : MonoBehaviour, IDisposable
    {
        [SerializeField] private EnemyDefinition _definition;
        [SerializeField] private Transform _muzzle;

        private EnemyBrain _brain;
        private EnemyComposite _composite;
        private CancellationTokenSource _cts;

        [Inject] private EnemyCompositeFactory _compositeFactory;
        [Inject] private EnemyBrainFactory _brainFactory;
        [Inject] private ITargetProvider _targets;

        private void Awake()
        {
            EnemyTeam team = GetComponent<EnemyTeam>();
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            EnemyContext context = new EnemyContext(transform, _targets, team.TeamId);

            _composite = _compositeFactory.Create(_definition, context, agent, _muzzle);
            _brain = _brainFactory.Create(_definition.BrainConfig, _composite);

            _cts = new CancellationTokenSource();
        }

        private void Start()
        {
            _brain.StartAsync(_cts.Token).Forget();
        }

        private void Update()
        {
            _brain.Tick(Time.deltaTime);
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
