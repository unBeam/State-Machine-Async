using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [field: SerializeField] public EnemyBrainConfig BrainConfig { get; private set; }
        [field: SerializeField] public EnemyMovementConfig Movement { get; private set; }
        [field: SerializeField] public EnemyAttackConfig Attack { get; private set; }
    }
}