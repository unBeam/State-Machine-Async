using Game.Features.Combat.Domain;
using UnityEngine;

namespace Game.Features.Combat.Application
{
    public sealed class PlayerTargetProvider : ITargetProvider
    {
        private readonly PlayerRegistry _registry;

        public PlayerTargetProvider(PlayerRegistry registry)
        {
            _registry = registry;
        }

        public Transform Current => _registry.Player;
    }
}