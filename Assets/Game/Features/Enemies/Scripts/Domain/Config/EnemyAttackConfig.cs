using UnityEngine;

namespace Game.Features.Enemies.Domain.Configs
{
    public abstract class EnemyAttackConfig : ScriptableObject
    {
        [SerializeField] private float _cooldownSeconds = 1.0f;

        public float CooldownSeconds
        {
            get { return _cooldownSeconds; }
        }
    }
}