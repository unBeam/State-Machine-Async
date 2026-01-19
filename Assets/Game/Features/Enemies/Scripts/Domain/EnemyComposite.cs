using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Domain.Modules;

namespace Game.Features.Enemies.Domain
{
    public sealed class EnemyComposite : IDisposable
    {
        private readonly IMovementModule _movement;
        private readonly IAttackModule _attack;

        public EnemyComposite(IMovementModule movement, IAttackModule attack)
        {
            _movement = movement;
            _attack = attack;
        }

        public IMovementModule Movement
        {
            get { return _movement; }
        }

        public IAttackModule Attack
        {
            get { return _attack; }
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            return UniTask.WhenAll(
                _movement.EnterAsync(cancellationToken),
                _attack.EnterAsync(cancellationToken)
            );
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            return UniTask.WhenAll(
                _movement.ExitAsync(cancellationToken),
                _attack.ExitAsync(cancellationToken)
            );
        }

        public void Tick(float deltaTime)
        {
            _movement.Tick(deltaTime);
            _attack.Tick(deltaTime);
        }

        public void Dispose()
        {
            IDisposable disposableMovement = _movement as IDisposable;
            disposableMovement?.Dispose();

            IDisposable disposableAttack = _attack as IDisposable;
            disposableAttack?.Dispose();
        }
    }
}