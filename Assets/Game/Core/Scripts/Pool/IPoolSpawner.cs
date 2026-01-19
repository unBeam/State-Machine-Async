namespace Game.Core.Scripts.Pool
{
    public interface IPoolSpawner<TValue, TParams>
    {
        TValue Spawn(TParams parameters);
        void Despawn(TValue value);
    }
}