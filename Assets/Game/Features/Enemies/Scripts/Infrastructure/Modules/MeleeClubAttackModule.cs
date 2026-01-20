using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Combat.Domain;
using Game.Features.Enemies.Domain.Modules;
using Game.Features.Enemies.Presentation.Unity;
using Game.Features.Enemies.Presentation.Unity.Configs;
using UnityEngine;

namespace Game.Features.Enemies.Infrastructure.Modules
{
    public sealed class MeleeClubAttackModule : IAttackModule
    {
        private readonly EnemyContext _context;
        private readonly MeleeClubConfig _config;

        private float _cooldownLeft;

        public MeleeClubAttackModule(EnemyContext context, MeleeClubConfig config)
        {
            _context = context;
            _config = config;
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

            Vector3 center = target.position;
            Collider[] hits = Physics.OverlapSphere(center, _config.HitRadius, _config.HitMask, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hits.Length; i++)
            {
                Transform root = hits[i].transform;

                IDamageable damageable = root.GetComponentInParent<IDamageable>();
                if (damageable == null)
                {
                    continue;
                }

                ITeamMember member = root.GetComponentInParent<ITeamMember>();
                if (member != null && member.TeamId == _context.TeamId)
                {
                    continue;
                }

                damageable.ApplyDamage(_config.Damage);
            }

            return UniTask.CompletedTask;
        }
    }
}
