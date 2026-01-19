using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Movement/NavMesh Movement Config")]
    public sealed class NavMeshMovementConfig : EnemyMovementConfig
    {
        [field: SerializeField] public float MoveSpeed { get; private set; } = 3.5f;
        [field: SerializeField] public float AngularSpeed { get; private set; } = 540f;
        [field: SerializeField] public float Acceleration { get; private set; } = 16f;
        [field: SerializeField] public float StoppingDistance { get; private set; } = 1.7f;
        [field: SerializeField] public float AttackRange { get; private set; } = 2.0f;
    }
}