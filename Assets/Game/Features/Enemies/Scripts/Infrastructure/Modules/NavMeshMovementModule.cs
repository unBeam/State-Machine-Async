using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Domain.Modules;
using Game.Features.Enemies.Presentation.Unity;
using Game.Features.Enemies.Presentation.Unity.Configs;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Features.Enemies.Application.Modules
{
    public sealed class NavMeshMovementModule : IMovementModule
    {
        private readonly EnemyContext _context;
        private readonly NavMeshAgent _agent;
        private readonly NavMeshMovementConfig _config;

        private float _attackRangeSqr;
        private float _updateThresholdSqr;

        public NavMeshMovementModule(EnemyContext context, NavMeshAgent agent, NavMeshMovementConfig config)
        {
            _context = context;
            _agent = agent;
            _config = config;

            float attackRange = _config.AttackRange;
            _attackRangeSqr = attackRange * attackRange;

            float updateThreshold = 0.25f;
            _updateThresholdSqr = updateThreshold * updateThreshold;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            _agent.speed = _config.MoveSpeed;
            _agent.angularSpeed = _config.AngularSpeed;
            _agent.acceleration = _config.Acceleration;
            _agent.stoppingDistance = _config.StoppingDistance;
            _agent.isStopped = false;

            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            _agent.isStopped = true;
            return UniTask.CompletedTask;
        }

        public void Tick(float deltaTime)
        {
            Transform target = _context.Target;
            if (target == null)
            {
                _agent.isStopped = true;
                return;
            }

            _agent.isStopped = false;

            if (_agent.hasPath == false)
            {
                _agent.SetDestination(target.position);
                return;
            }

            Vector3 delta = _agent.destination - target.position;
            if (delta.sqrMagnitude > _updateThresholdSqr)
            {
                _agent.SetDestination(target.position);
            }
        }

        public bool IsInAttackRange()
        {
            Transform target = _context.Target;
            if (target == null)
            {
                return false;
            }

            Vector3 delta = target.position - _context.Self.position;
            return delta.sqrMagnitude <= _attackRangeSqr;
        }

        public void Stop()
        {
            _agent.isStopped = true;
        }
    }
}
