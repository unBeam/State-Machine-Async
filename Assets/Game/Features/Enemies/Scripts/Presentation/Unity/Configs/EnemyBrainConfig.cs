using UnityEngine;
using Game.Features.Enemies.Application;

namespace Game.Features.Enemies.Presentation.Unity.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Enemy Brain Config")]
    public sealed class EnemyBrainConfig : ScriptableObject
    {
        [field: SerializeField] public EnemyStateID InitialStateId { get; private set; } = EnemyStateID.Idle;

        [field: SerializeField]
        public EnemyStateID[] RequiredStates { get; private set; } =
        {
            EnemyStateID.Idle,
            EnemyStateID.Engage,
            EnemyStateID.Attack,
            EnemyStateID.Dead
        };
    }
}