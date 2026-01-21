using Game.Features.Combat.Domain;
using Game.Features.Enemies.Presentation.Unity.Configs;
using UnityEngine;

namespace Game.Features.Combat.Domain
{
    public sealed class BulletSpawnParamsBuilder
    {
        public BulletSpawnParams Build(
            Transform muzzle,
            Transform target,
            int ownerTeamId,
            RangedWeaponConfig config)
        {
            Vector3 dir = GetDirection(muzzle, target);
            Vector3 velocity = dir * config.ProjectileSpeed;
            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

            BulletShotParams shot = new BulletShotParams(
                muzzle.position,
                rotation,
                velocity,
                ownerTeamId
            );

            BulletSpecParams spec = new BulletSpecParams(
                config.BulletLifeTimeSeconds,
                config.BulletDamage,
                config.FriendlyFire
            );

            return new BulletSpawnParams(shot, spec);
        }

        private Vector3 GetDirection(Transform muzzle, Transform target)
        {
            Vector3 delta = target.position - muzzle.position;
            if (delta.sqrMagnitude <= 0.000001f)
            {
                return muzzle.forward;
            }

            return delta.normalized;
        }
    }
}