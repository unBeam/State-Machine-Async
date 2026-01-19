using Zenject;
using Game.Features.Combat;

namespace Game.Core.Scripts.Pooling
{
    public sealed class BulletMemoryPool : MonoMemoryPool<Bullet>
    {
        protected override void OnCreated(Bullet item)
        {
            item.gameObject.SetActive(false);
        }
    }
}