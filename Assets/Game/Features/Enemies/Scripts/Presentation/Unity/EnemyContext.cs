using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity
{
    public sealed class EnemyContext
    {
        private readonly Transform _self;
        private readonly Transform _target;

        public EnemyContext(Transform self, Transform target)
        {
            _self = self;
            _target = target;
        }

        public Transform Self
        {
            get { return _self; }
        }

        public Transform Target
        {
            get { return _target; }
        }
    }
}