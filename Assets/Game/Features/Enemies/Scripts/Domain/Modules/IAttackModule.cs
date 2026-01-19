using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Features.Enemies.Domain.Modules
{
    public interface IAttackModule
    {
        UniTask EnterAsync(CancellationToken cancellationToken);
        UniTask ExitAsync(CancellationToken cancellationToken);
        void Tick(float deltaTime);
        bool CanAttack();
        UniTask AttackAsync(CancellationToken cancellationToken);
    }
}