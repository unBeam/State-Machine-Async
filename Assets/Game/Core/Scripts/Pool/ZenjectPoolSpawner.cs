using Game.Core.Scripts.Pool;
using Zenject;

namespace Game.Core.Scripts.Pooling
{
    public sealed class ZenjectPoolSpawner<TValue, TParams> : IPoolSpawner<TValue, TParams>
        where TValue : class
    {
        private readonly IMemoryPool<TParams, TValue> _pool;

        public ZenjectPoolSpawner(IMemoryPool<TParams, TValue> pool)
        {
            _pool = pool;
        }

        public TValue Spawn(TParams p)
        {
            return _pool.Spawn(p);
        }

        public void Despawn(TValue value)
        {
            _pool.Despawn(value);
        }
    }
}