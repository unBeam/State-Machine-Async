using Game.Features.PlayerUI.Application;
using Game.Features.PlayerUI.Domain;
using Game.Features.PlayerUI.Presentation.Unity;
using Zenject;

namespace Game.Features.PlayerUI.Presentation.Unity.Installers
{
    public sealed class PlayerUIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerHpView>().To<PlayerHpBarView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHpPresenter>().AsSingle();
        }
    }
}