using System;
using Game.Features.Player.Domain;
using Game.Features.PlayerUI.Domain;
using R3;
using Zenject;

namespace Game.Features.PlayerUI.Application
{
    public sealed class PlayerHpPresenter : IInitializable, IDisposable
    {
        private readonly PlayerHealth _health;
        private readonly IPlayerHpView _view;

        private IDisposable _sub;

        public PlayerHpPresenter(PlayerHealth health, IPlayerHpView view)
        {
            _health = health;
            _view = view;
        }

        public void Initialize()
        {
            _sub = _health.CurrentHp.Subscribe(hp =>
            {
                _view.SetNormalized(Normalize(hp, _health.MaxHp));
            });
        }

        public void Dispose()
        {
            _sub?.Dispose();
            _sub = null;
        }

        private static float Normalize(float current, float max)
        {
            if (max <= 0.0f)
            {
                return 0.0f;
            }

            float value = current / max;

            if (value < 0.0f)
            {
                return 0.0f;
            }

            if (value > 1.0f)
            {
                return 1.0f;
            }

            return value;
        }
    }
}