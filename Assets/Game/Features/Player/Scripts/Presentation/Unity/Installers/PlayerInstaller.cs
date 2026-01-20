using Game.Features.Player.Domain;
using Zenject;
using UnityEngine;

namespace Game.Features.Player.Presentation.Unity.Installers
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<PlayerConfig>().FromInstance(_config).AsSingle();

            Container.Bind<PlayerHealth>().AsSingle()
                .WithArguments(_config.MaxHp);
        }
    }
}