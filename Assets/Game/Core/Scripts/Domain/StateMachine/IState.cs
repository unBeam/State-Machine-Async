using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.Scripts.Domain.StateMachine
{
    public interface IState
    {
        UniTask EnterAsync(CancellationToken cancellationToken);
        UniTask ExitAsync(CancellationToken cancellationToken);
        void Tick(float deltaTime);
    }
}