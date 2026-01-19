using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Combat;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Domain.Modules;
using Game.Features.Enemies.Presentation.Unity;
using Game.Features.Enemies.Presentation.Unity.Configs;
using UnityEngine;

namespace Game.Features.Enemies.Application.Modules
{
    public sealed class RangedWeaponAttackModule : IAttackModule
    {
        private readonly EnemyContext _context;
        private readonly Transform _muzzle;
        private readonly RangedWeaponConfig _config;
        private readonly BulletService _bullets;

        private float _cooldownLeft;

        public RangedWeaponAttackModule(
            EnemyContext context,
            Transform muzzle,
            RangedWeaponConfig config,
            BulletService bullets)
        {
            _context = context;
            _muzzle = muzzle;
            _config = config;
            _bullets = bullets;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            _cooldownLeft = 0f;
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public void Tick(float deltaTime)
        {
            if (_cooldownLeft > 0f)
            {
                _cooldownLeft = Math.Max(0f, _cooldownLeft - deltaTime);
            }
        }

        public bool CanAttack()
        {
            Transform target = _context.Target;
            if (target == null)
            {
                return false;
            }

            float dist = Vector3.Distance(_context.Self.position, target.position);
            if (dist > _config.MaxShootDistance)
            {
                return false;
            }

            return _cooldownLeft <= 0f;
        }

        public UniTask AttackAsync(CancellationToken cancellationToken)
        {
            _cooldownLeft = _config.CooldownSeconds;

            Transform target = _context.Target;
            if (target == null)
            {
                return UniTask.CompletedTask;
            }

            if (_muzzle == null)
            {
                return UniTask.CompletedTask;
            }

            Vector3 dir = (target.position - _muzzle.position).normalized;
            Vector3 velocity = dir * _config.ProjectileSpeed;

            BulletSpawnParams bulletParams = new BulletSpawnParams(
                _muzzle.position,
                Quaternion.LookRotation(dir, Vector3.up),
                velocity,
                _config.BulletLifeTimeSeconds
            );

            _bullets.Spawn(bulletParams);

            return UniTask.CompletedTask;
        }
    }
}
