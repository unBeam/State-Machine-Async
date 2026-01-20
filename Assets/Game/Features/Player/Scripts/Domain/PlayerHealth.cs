using System;
using Game.Features.Combat.Domain;
using R3;

namespace Game.Features.Player.Domain
{
    public sealed class PlayerHealth : IDamageable, IDisposable
    {
        private readonly ReactiveProperty<float> _currentHp;
        private readonly float _maxHp;
        private readonly Subject<Unit> _died;
        
        public float MaxHp => _maxHp;
        public ReadOnlyReactiveProperty<float> CurrentHp => _currentHp;
        public Observable<Unit> Died => _died;
        public bool IsDead => _currentHp.Value <= 0.0f;

        public PlayerHealth(float maxHp)
        {
            if (maxHp <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHp));
            }

            _maxHp = maxHp;
            _currentHp = new ReactiveProperty<float>(maxHp);
            _died = new Subject<Unit>();
        }

        public void ApplyDamage(float value)
        {
            if (value <= 0.0f || IsDead)
            {
                return;
            }

            float next = _currentHp.Value - value;
            if (next < 0.0f)
            {
                next = 0.0f;
            }

            _currentHp.Value = next;

            if (next <= 0.0f)
            {
                _died.OnNext(Unit.Default);
            }
        }

        public void Dispose()
        {
            _currentHp.Dispose();
            _died.Dispose();
        }
    }
}