using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Pooling;
using Game.Features.Combat.Domain;

namespace Game.Features.Combat
{
    public sealed class BulletService
    {
        private readonly BulletMemoryPool _pool;
        private readonly Dictionary<Bullet, CancellationTokenSource> _lifeTokens;
        private readonly Dictionary<Bullet, Action<Bullet>> _hitHandlers;

        public BulletService(BulletMemoryPool pool)
        {
            _pool = pool;
            _lifeTokens = new Dictionary<Bullet, CancellationTokenSource>();
            _hitHandlers = new Dictionary<Bullet, Action<Bullet>>();
        }

        public Bullet Spawn(BulletSpawnParams bulletParams)
        {
            Bullet bullet = _pool.Spawn(bulletParams);
            bullet.gameObject.SetActive(true);

            UnsubscribeHit(bullet);
            SubscribeHit(bullet);

            StartLifetime(bullet, bullet.LifeSeconds);

            return bullet;
        }

        private void SubscribeHit(Bullet bullet)
        {
            Action<Bullet> handler = OnBulletHit;
            _hitHandlers.Add(bullet, handler);
            bullet.Hit += handler;
        }

        private void UnsubscribeHit(Bullet bullet)
        {
            Action<Bullet> handler;
            if (_hitHandlers.TryGetValue(bullet, out handler) == false)
            {
                return;
            }

            _hitHandlers.Remove(bullet);
            bullet.Hit -= handler;
        }

        private void StartLifetime(Bullet bullet, float lifeSeconds)
        {
            StopLifetime(bullet);

            CancellationTokenSource cts = new CancellationTokenSource();
            _lifeTokens[bullet] = cts;

            DespawnAfterDelayAsync(bullet, lifeSeconds, cts.Token).Forget();
        }

        private void StopLifetime(Bullet bullet)
        {
            CancellationTokenSource cts;
            if (_lifeTokens.TryGetValue(bullet, out cts) == false)
            {
                return;
            }

            _lifeTokens.Remove(bullet);
            cts.Cancel();
            cts.Dispose();
        }

        private async UniTaskVoid DespawnAfterDelayAsync(Bullet bullet, float lifeSeconds, CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(lifeSeconds), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            Despawn(bullet);
        }

        private void OnBulletHit(Bullet bullet)
        {
            Despawn(bullet);
        }

        private void Despawn(Bullet bullet)
        {
            StopLifetime(bullet);
            UnsubscribeHit(bullet);
            
            bullet.gameObject.SetActive(false);

            _pool.Despawn(bullet);
        }
    }
}
