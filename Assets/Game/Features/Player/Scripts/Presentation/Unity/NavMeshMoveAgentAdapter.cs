using System;
using Game.Features.Player.Application;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Features.Player.Presentation.Unity
{
    public sealed class NavMeshMoveAgentAdapter : IPlayerMoveAgent
    {
        private readonly NavMeshAgent _agent;

        public NavMeshMoveAgentAdapter(NavMeshAgent agent)
        {
            _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public void SetDestination(Vector3 point)
        {
            _agent.SetDestination(point);
        }
    }
}