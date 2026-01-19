using UnityEngine;

namespace Game.Features.Enemies.Domain.Configs
{
    [CreateAssetMenu(menuName = "Game/Enemies/Movement/NavMesh Movement Config")]
    public sealed class NavMeshMovementConfig : EnemyMovementConfig
    {
        [SerializeField] private float _moveSpeed = 3.5f;
        [SerializeField] private float _angularSpeed = 540f;
        [SerializeField] private float _acceleration = 16f;
        [SerializeField] private float _stoppingDistance = 1.7f;
        [SerializeField] private float _attackRange = 2.0f;

        public float MoveSpeed
        {
            get { return _moveSpeed; }
        }

        public float AngularSpeed
        {
            get { return _angularSpeed; }
        }

        public float Acceleration
        {
            get { return _acceleration; }
        }

        public float StoppingDistance
        {
            get { return _stoppingDistance; }
        }

        public float AttackRange
        {
            get { return _attackRange; }
        }
    }
}