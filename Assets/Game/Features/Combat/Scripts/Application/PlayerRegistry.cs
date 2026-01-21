using UnityEngine;

namespace Game.Features.Combat.Application
{
    public sealed class PlayerRegistry
    {
        private Transform _player;

        public Transform Player => _player;

        public void Register(Transform player)
        {
            _player = player;
        }

        public void Unregister(Transform player)
        {
            if (ReferenceEquals(_player, player))
            {
                _player = null;
            }
        }
    }
}