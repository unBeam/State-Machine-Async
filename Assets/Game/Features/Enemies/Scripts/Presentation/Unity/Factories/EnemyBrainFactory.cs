using System;
using System.Collections.Generic;
using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Application;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Presentation.Unity.Configs;

namespace Game.Features.Enemies.Presentation.Unity.Factories
{
    public sealed class EnemyBrainFactory
    {
        private readonly EnemyStateRegistry _registry;

        public EnemyBrainFactory(EnemyStateRegistry registry)
        {
            _registry = registry;
        }

        public EnemyBrain Create(EnemyBrainConfig config, EnemyComposite composite)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (composite == null)
            {
                throw new ArgumentNullException(nameof(composite));
            }

            EnemyStateID[] requiredStates = config.RequiredStates;
            if (requiredStates == null || requiredStates.Length == 0)
            {
                throw new InvalidOperationException("RequiredStates is empty.");
            }

            ValidateUnique(requiredStates);
            EnsureContains(requiredStates, EnemyStateID.Dead);

            EnemyStateSwitcher switcher = new EnemyStateSwitcher();

            Dictionary<EnemyStateID, IState> states = new Dictionary<EnemyStateID, IState>(requiredStates.Length);

            for (int i = 0; i < requiredStates.Length; i++)
            {
                EnemyStateID id = requiredStates[i];
                states[id] = _registry.Create(id, composite, switcher);
            }

            EnemyBrain brain = new EnemyBrain(states, config.InitialStateId);
            switcher.Bind(brain);
            return brain;
        }

        private static void ValidateUnique(EnemyStateID[] ids)
        {
            HashSet<EnemyStateID> set = new HashSet<EnemyStateID>();

            for (int i = 0; i < ids.Length; i++)
            {
                EnemyStateID id = ids[i];

                if (!set.Add(id))
                {
                    throw new InvalidOperationException("Duplicate state id in config: " + id);
                }
            }
        }

        private static void EnsureContains(EnemyStateID[] ids, EnemyStateID required)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == required)
                {
                    return;
                }
            }

            throw new InvalidOperationException("Config does not contain required state: " + required);
        }
    }
}
