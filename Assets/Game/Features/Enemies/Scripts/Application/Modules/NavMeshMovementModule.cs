using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Domain.Configs;
using Game.Features.Enemies.Domain.Modules;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Features.Enemies.Application.Modules
{
    public sealed class NavMeshMovementModule : IMovementModule
    {
        private readonly EnemyContext _context;
        private readonly NavMeshAgent _agent;
        private readonly NavMeshMovementConfig _config;

        public NavMeshMovementModule(EnemyContext context, NavMeshAgent agent, NavMeshMovementConfig config)
        {
            _context = context;
            _agent = agent;
            _config = config;
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

            float updateThreshold = 0.25f;
            if (_agent.hasPath == false || Vector3.Distance(_agent.destination, target.position) > updateThreshold)
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

            float dist = Vector3.Distance(_context.Self.position, target.position);
            return dist <= _config.AttackRange;
        }

        public void Stop()
        {
            _agent.isStopped = true;
        }
    }
}
