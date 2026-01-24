using System;
using System.Collections.Generic;
using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Application;
using Game.Features.Enemies.Application.States;
using Game.Features.Enemies.Domain;

namespace Game.Features.Enemies.Presentation.Unity.Factories
{
    public sealed class EnemyStateRegistry
    {
        private readonly Dictionary<EnemyStateID, Func<EnemyComposite, IEnemyStateSwitcher, IState>> _builders;

        public EnemyStateRegistry()
        {
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
            if (_builders.TryGetValue(id, out builder) == false)
            {
                throw new InvalidOperationException("No builder registered for state: " + id);
            }

            return builder(composite, switcher);
        }

        private static IState BuildIdle(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return new EnemyIdleState();
        }

        private static IState BuildEngage(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return new EnemyEngageState(composite, switcher);
        }

        private static IState BuildAttack(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return new EnemyAttackState(composite, switcher);
        }

        private static IState BuildDead(EnemyComposite composite, IEnemyStateSwitcher switcher)
        {
            return new EnemyDeadState();
        }
    }
}