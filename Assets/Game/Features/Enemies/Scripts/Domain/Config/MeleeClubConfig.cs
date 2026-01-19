using UnityEngine;

namespace Game.Features.Enemies.Domain.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Attack/Melee Club Config")]
    public sealed class MeleeClubConfig : EnemyAttackConfig
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _hitRadius = 1.2f;
        [SerializeField] private LayerMask _hitMask;

        public float Damage
        {
            get { return _damage; }
        }

        public float HitRadius
        {
            get { return _hitRadius; }
        }

        public LayerMask HitMask
        {
            get { return _hitMask; }
        }
    }
}