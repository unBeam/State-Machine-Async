using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Features.Enemies.Domain;
using Game.Features.Enemies.Domain.Modules;

namespace Game.Features.Enemies.Application.Modules
{
    public sealed class MeleeAttackModule : IAttackModule
    {
        private readonly EnemyContext _context;
        private readonly float _cooldownSeconds;
        private float _cooldownLeft;

        public MeleeAttackModule(EnemyContext context, float cooldownSeconds)
        {
            _context = context;
            _cooldownSeconds = cooldownSeconds;
        }

        public UniTask EnterAsync(CancellationToken cancellationToken)
        {
            _cooldownLeft = 0f;
            return UniTask.CompletedTask;
        }

        public UniTask ExitAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public void Tick(float deltaTime)
        {
            if (_cooldownLeft > 0f)
            {
                _cooldownLeft = Math.Max(0f, _cooldownLeft - deltaTime);
            }
        }

        public bool CanAttack()
        {
            return _cooldownLeft <= 0f;
        }

        public UniTask AttackAsync(CancellationToken cancellationToken)
        {
            _cooldownLeft = _cooldownSeconds;
            return UniTask.CompletedTask;
        }
    }
}