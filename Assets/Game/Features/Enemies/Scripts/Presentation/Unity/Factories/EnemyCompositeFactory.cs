using System;
using Game.Features.Combat;
using Game.Features.Enemies.Application.Modules;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Domain.Modules;
using Game.Features.Enemies.Infrastructure.Modules;
using Game.Features.Enemies.Presentation.Unity.Configs;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Features.Enemies.Presentation.Unity.Factories
{
    public sealed class EnemyCompositeFactory
    {
        private readonly BulletService _bullets;

        public EnemyCompositeFactory(BulletService bullets)
        {
            _bullets = bullets;
        }

        public EnemyComposite Create(EnemyDefinition definition, EnemyContext context, NavMeshAgent agent, Transform muzzle)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            IMovementModule movement = CreateMovement(definition.Movement, context, agent);
            IAttackModule attack = CreateAttack(definition.Attack, context, muzzle);

            return new EnemyComposite(movement, attack);
        }

        private static IMovementModule CreateMovement(EnemyMovementConfig config, EnemyContext context, NavMeshAgent agent)
        {
            NavMeshMovementConfig navMeshConfig = config as NavMeshMovementConfig;
            if (navMeshConfig != null)
            {
                if (agent == null)
                {
                    throw new InvalidOperationException("NavMeshAgent is required for NavMeshMovementConfig.");
                }

                return new NavMeshMovementModule(context, agent, navMeshConfig);
            }

            throw new InvalidOperationException("Unsupported movement config: " + config);
        }

        private IAttackModule CreateAttack(EnemyAttackConfig config, EnemyContext context, Transform muzzle)
        {
            RangedWeaponConfig ranged = config as RangedWeaponConfig;
            if (ranged != null)
            {
                return new RangedWeaponAttackModule(context, muzzle, ranged, _bullets);
            }

            MeleeClubConfig melee = config as MeleeClubConfig;
            if (melee != null)
            {
                return new MeleeClubAttackModule(context, melee);
            }

            throw new InvalidOperationException("Unsupported attack config: " + config);
        }
    }
}
