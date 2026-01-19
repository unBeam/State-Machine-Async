using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Application;

namespace Game.Features.Enemies.Domain
{
    public interface IEnemyStateSwitcher
    {
        UniTask SetStateAsync(EnemyStateID id, CancellationToken cancellationToken);
    }
}