using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Features.Enemies.Domain.Modules
{
    public interface IMovementModule
    {
        UniTask EnterAsync(CancellationToken cancellationToken);
        UniTask ExitAsync(CancellationToken cancellationToken);
        void Tick(float deltaTime);
        bool IsInAttackRange();
        void Stop();
    }
}