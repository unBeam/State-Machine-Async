using System;
using System.Collections.Generic;
using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Application;
using Game.Features.Enemies.Application.States;
using Game.Features.Enemies.Domain;
using Zenject;

namespace Game.Features.Enemies.Presentation.Unity.Factories
{
    public sealed class EnemyStateRegistry
    {
        private readonly DiContainer _container;
        private readonly Dictionary<EnemyStateID, Func<EnemyComposite, IEnemyStateSwitcher, IState>> _builders;

        public EnemyStateRegistry(DiContainer container)
        {
            _container = container;

            _builders = new Dictionary<EnemyStateID, Func<EnemyComposite, IEnemyStateSwitcher, IState>>
            {
                { EnemyStateID.Idle, BuildIdle },
                { EnemyStateID.Engage, BuildEngage },
                { EnemyStateID.Attack, BuildAttack },
                { EnemyStateID.Dead, BuildDead }
            };
        }

        public IState Create(EnemyStateID id, EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            Func<EnemyComposite, IEnemyStateSwitcher, IState> builder;
            if (!_builders.TryGetValue(id, out builder))
            {
                throw new InvalidOperationException("No builder registered for state: " + id);
            }

            return builder(composite, switcher);
        }

        private IState BuildIdle(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return _container.Instantiate<EnemyIdleState>();
        }

        private IState BuildEngage(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return new EnemyEngageState(composite, switcher);
        }

        private IState BuildAttack(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return new EnemyAttackState(composite, switcher);
        }

        private IState BuildDead(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return _container.Instantiate<EnemyDeadState>();
        }
    }
}
