using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Attack/Melee Club Config")]
    public sealed class MeleeClubConfig : EnemyAttackConfig
    {
        [field: SerializeField] public float Damage { get; private set; } = 10f;
        [field: SerializeField] public float HitRadius { get; private set; } = 1.2f;
        [field: SerializeField] public LayerMask HitMask { get; private set; }
    }
}