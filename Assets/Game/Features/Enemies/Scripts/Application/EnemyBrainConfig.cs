using UnityEngine;

namespace Game.Features.Enemies.Application
{
    [CreateAssetMenu(menuName = "Game/Enemies/Enemy Brain Config")]
    public sealed class EnemyBrainConfig : ScriptableObject
    {
        [SerializeField] private EnemyStateID _initialStateId = EnemyStateID.Idle;
        [SerializeField] private EnemyStateID[] _requiredStates =
        {
            EnemyStateID.Idle,
            EnemyStateID.Engage,
            EnemyStateID.Attack,
            EnemyStateID.Dead
        };

        public EnemyStateID InitialStateId
        {
            get { return _initialStateId; }
        }

        public EnemyStateID[] RequiredStates
        {
            get { return _requiredStates; }
        }
    }
}