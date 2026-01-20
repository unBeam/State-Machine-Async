using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Application;

namespace Game.Features.Enemies.Domain
{
    public sealed class EnemyStateSwitcher : IEnemyStateSwitcher
    {
        private EnemyBrain _brain;

        public void Bind(EnemyBrain brain)
        {
            if (brain == null)
            {
                throw new ArgumentNullException(nameof(brain));
            }

            _brain = brain;
        }

        public UniTask SetStateAsync(EnemyStateID id, CancellationToken cancellationToken)
        {
            if (_brain == null)
            {
                throw new InvalidOperationException("EnemyStateSwitcher is not bound to EnemyBrain.");
            }

            return _brain.SetStateAsync(id, cancellationToken);
        }
    }
}