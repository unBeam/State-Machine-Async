using System;
using System.Collections.Generic;
using Game.Core.Scripts.Domain.StateMachine;
using Zenject;

namespace Game.Features.Enemies.Application
{
    public sealed class EnemyBrainFactory
    {
        private readonly DiContainer _container;
        private readonly EnemyBrainConfig _config;

        public EnemyBrainFactory(DiContainer container, EnemyBrainConfig config)
        {
            _container = container;
            _config = config;
        }

        public EnemyBrain Create()
        {
            return Create(_config);
        }

        public EnemyBrain Create(EnemyBrainConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            EnemyStateID[] requiredStates = config.RequiredStates;
            if (requiredStates == null || requiredStates.Length == 0)
            {
                throw new InvalidOperationException("RequiredStates is empty.");
            }

            ValidateUnique(requiredStates);
            EnsureContains(requiredStates, EnemyStateID.Dead);

            Dictionary<EnemyStateID, IState> states = new Dictionary<EnemyStateID, IState>(requiredStates.Length);

            for (int i = 0; i < requiredStates.Length; i++)
            {
                EnemyStateID id = requiredStates[i];

                IState state = _container.ResolveId<IState>(id);
                if (state == null)
                {
                    throw new InvalidOperationException("Resolved state is null: " + id);
                }

                states[id] = state;
            }

            return new EnemyBrain(states, config.InitialStateId);
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
