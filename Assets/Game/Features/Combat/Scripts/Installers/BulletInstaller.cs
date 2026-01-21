using UnityEngine;
using Zenject;
using Game.Features.Combat;
using Game.Core.Scripts.Pooling;
using Game.Features.Combat.Domain;

namespace Game.Features.Combat.Installers
{
    public sealed class BulletInstaller : MonoInstaller
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int _initialSize = 32;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Bullet, BulletMemoryPool>()
                .WithInitialSize(_initialSize)
                .FromComponentInNewPrefab(bulletPrefab)
                .UnderTransformGroup("Projectiles");

            Container.Bind<BulletService>().AsSingle();
            Container.Bind<BulletSpawnParamsBuilder>().AsSingle();
        }

    }
}