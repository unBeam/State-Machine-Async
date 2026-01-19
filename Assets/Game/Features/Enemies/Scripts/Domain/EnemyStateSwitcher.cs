using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Application;

namespace Game.Features.Enemies.Domain
{
    public sealed class EnemyStateSwitcher : IEnemyStateSwitcher
    {
        private readonly Func<EnemyBrain> _brainGetter;

        public EnemyStateSwitcher(Func<EnemyBrain> brainGetter)
        {
            _brainGetter = brainGetter;
        }

        public UniTask SetStateAsync(EnemyStateID id, CancellationToken cancellationToken)
        {
            EnemyBrain brain = _brainGetter.Invoke();
            if (brain == null)
            {
                return UniTask.CompletedTask;
            }

            return brain.SetStateAsync(id, cancellationToken);
        }
    }
}