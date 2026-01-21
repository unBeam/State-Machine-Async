using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Attack/Ranged Weapon Config")]
    public sealed class RangedWeaponConfig : EnemyAttackConfig
    {
        [field: SerializeField] public GameObject ProjectilePrefab { get; private set; }
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: SerializeField] public float MaxShootDistance { get; private set; }
        [field: SerializeField] public float BulletLifeTimeSeconds { get; private set; }
        [field: SerializeField] public float BulletDamage { get; private set; }
        [field: SerializeField] public bool FriendlyFire { get; private set; }
    }
}