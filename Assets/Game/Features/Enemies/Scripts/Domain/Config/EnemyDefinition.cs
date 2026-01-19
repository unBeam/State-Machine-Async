using Game.Features.Enemies.Application;
using UnityEngine;

namespace Game.Features.Enemies.Domain.Configs
{
    public enum EnemyMovementType
    {
        Walker = 0,
        Flyer = 1
    }

    public enum EnemyAttackType
    {
        Melee = 0,
        Ranged = 1,
        Grenade = 2
    }

    [CreateAssetMenu(menuName = "Game/Enemies/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [SerializeField] private EnemyBrainConfig _brainConfig;
        [SerializeField] private EnemyMovementConfig _movement;
        [SerializeField] private EnemyAttackConfig _attack;

        public EnemyBrainConfig BrainConfig
        {
            get { return _brainConfig; }
        }

        public EnemyMovementConfig Movement
        {
            get { return _movement; }
        }

        public EnemyAttackConfig Attack
        {
            get { return _attack; }
        }
    }
}