using System;
using R3;
using UnityEngine;

namespace Game.Features.Combat
{
    public sealed class Bullet : MonoBehaviour, IDisposable
    {
        [SerializeField] private Rigidbody _rigidbody;

        private readonly Subject<Collision> _hit = new Subject<Collision>();

        public Observable<Collision> Hit => _hit;

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
            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _hit.OnNext(collision);
        }

        public void Dispose()
        {
            _hit.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}