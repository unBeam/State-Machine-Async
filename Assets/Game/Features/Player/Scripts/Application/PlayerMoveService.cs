using System;
using UnityEngine;

namespace Game.Features.Player.Application
{
    public sealed class PlayerMoveService
    {
        private readonly IPlayerMoveAgent _agent;

        public PlayerMoveService(IPlayerMoveAgent agent)
        {
            _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public void MoveTo(Vector3 point)
        {
            _agent.SetDestination(point);
        }
    }
}