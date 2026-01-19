using System;
using UnityEngine;

namespace Game.Features.Combat
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public event Action<Bullet, Collision> Hit;

        public void Apply(BulletSpawnParams bulletParams)
        {
            transform.SetPositionAndRotation(bulletParams.Position, bulletParams.Rotation);

            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = bulletParams.Velocity;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        public void ResetState()
        {
            Hit = null;

            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Action<Bullet, Collision> handler = Hit;
            if (handler == null)
            {
                return;
            }

            handler(this, collision);
        }
    }
}