using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity.Configs
{
    public abstract class EnemyAttackConfig : ScriptableObject
    {
        [field: SerializeField] public float CooldownSeconds { get; private set; } = 1.0f;
    }
}