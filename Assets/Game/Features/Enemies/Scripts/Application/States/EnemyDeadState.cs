using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Domain.StateMachine;

namespace Game.Features.Enemies.Application.States
{
    public sealed class EnemyDeadState : IState
    {
        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public void Tick(float deltaTime)
        {
        }
    }
}