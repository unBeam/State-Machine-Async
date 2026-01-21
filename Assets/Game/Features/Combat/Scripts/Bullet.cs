using System;
using Game.Features.Combat.Domain;
using UnityEngine;

namespace Game.Features.Combat
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet> Hit;

        [SerializeField] private Rigidbody _rigidbody;

        private BulletSpawnParams _params;

        public float LifeSeconds
        {
            get { return _params.Spec.LifeSeconds; }
        }

        public void Apply(BulletSpawnParams bulletParams)
        {
            _params = bulletParams;

            transform.SetPositionAndRotation(bulletParams.Shot.Position, bulletParams.Shot.Rotation);

            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = bulletParams.Shot.Velocity;
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

            _params = default(BulletSpawnParams);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
            {
                return;
            }

            IDamageable damageable;
            if (collision.gameObject.TryGetComponent<IDamageable>(out damageable) == false)
            {
                Hit?.Invoke(this);
                return;
            }

            if (_params.Spec.FriendlyFire == false)
            {
                ITeamMember teamMember;
                if (collision.gameObject.TryGetComponent<ITeamMember>(out teamMember))
                {
                    if (teamMember.TeamId == _params.Shot.OwnerTeamId)
                    {
                        Hit?.Invoke(this);
                        return;
                    }
                }
            }

            damageable.ApplyDamage(_params.Spec.Damage);
            Hit?.Invoke(this);
        }
    }
}
