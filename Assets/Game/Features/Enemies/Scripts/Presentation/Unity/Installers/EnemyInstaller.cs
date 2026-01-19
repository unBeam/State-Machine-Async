using Game.Core.Scripts.Domain.StateMachine;
using Game.Features.Enemies.Application;
using Game.Features.Enemies.Application.States;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Presentation.Unity.Configs;
using Game.Features.Enemies.Presentation.Unity.Factories;
using UnityEngine;
using Zenject;

namespace Game.Features.Enemies.Presentation.Unity.Installers
{
    public sealed class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemyBrainConfig _brainConfig;

        public override void InstallBindings()
        {
            Container.Bind<EnemyBrainConfig>().FromInstance(_brainConfig).AsSingle();
            Container.Bind<EnemyBrainFactory>().AsTransient();
            Container.Bind<EnemyCompositeFactory>().AsSingle();
   
            Container.Bind<IState>().WithId(EnemyStateID.Idle).To<EnemyIdleState>().AsTransient();
            Container.Bind<IState>().WithId(EnemyStateID.Engage).To<EnemyEngageState>().AsTransient();
            Container.Bind<IState>().WithId(EnemyStateID.Attack).To<EnemyAttackState>().AsTransient();
            Container.Bind<IState>().WithId(EnemyStateID.Dead).To<EnemyDeadState>().AsTransient();
        }
    }
}