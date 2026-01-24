using Game.Features.Enemies.Presentation.Unity.Factories;
using Zenject;

namespace Game.Features.Enemies.Presentation.Unity.Installers
{
    public sealed class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyCompositeFactory>().AsTransient();

            Container.Bind<EnemyStateRegistry>().AsSingle();
            Container.Bind<EnemyBrainFactory>().AsTransient();
        }
    }
}