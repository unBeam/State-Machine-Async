using Game.Features.Combat.Application;
using Game.Features.Combat.Domain;
using Zenject;

namespace Game.Features.Combat.Presentation.Unity.Installers
{
    public sealed class CombatInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerRegistry>().AsSingle();
            Container.Bind<ITargetProvider>().To<PlayerTargetProvider>().AsSingle();
        }
    }
}