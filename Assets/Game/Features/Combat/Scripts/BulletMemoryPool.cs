using Game.Features.Combat;
using Game.Features.Combat.Domain;
using Zenject;

namespace Game.Core.Scripts.Pooling
{
    public sealed class BulletMemoryPool : MonoMemoryPool<BulletSpawnParams, Bullet>
    {
        protected override void OnCreated(Bullet item)
        {
            item.gameObject.SetActive(false);
        }

        protected override void OnDestroyed(Bullet item)
        {
            item.gameObject.SetActive(false);
        }
    }
}