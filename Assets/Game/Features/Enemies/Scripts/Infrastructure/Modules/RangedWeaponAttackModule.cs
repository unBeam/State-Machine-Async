using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Combat;
using Game.Features.Combat.Domain;
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
        private readonly BulletSpawnParamsBuilder _builder;

        private float _cooldownLeft;

        public RangedWeaponAttackModule(
            EnemyContext context,
            Transform muzzle,
            RangedWeaponConfig config,
            BulletService bullets,
            BulletSpawnParamsBuilder builder)
        {
            _context = context;
            _muzzle = muzzle;
            _config = config;
            _bullets = bullets;
            _builder = builder;
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

            BulletSpawnParams bulletParams = _builder.Build(
                _muzzle,
                target,
                _context.TeamId,
                _config
            );

            _bullets.Spawn(bulletParams);

            return UniTask.CompletedTask;
        }
    }
}
