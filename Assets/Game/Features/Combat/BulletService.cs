using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Scripts.Pooling;
using Game.Features.Combat;
using Zenject;
using UnityEngine;

namespace Game.Features.Combat
{
    public sealed class BulletService
    {
        private readonly BulletMemoryPool _pool;
        private readonly Dictionary<int, CancellationTokenSource> _lifeTokens;

        public BulletService(BulletMemoryPool pool)
        {
            _pool = pool;
            _lifeTokens = new Dictionary<int, CancellationTokenSource>();
        }

        public Bullet Spawn(BulletSpawnParams bulletParams)
        {
            Bullet bullet = _pool.Spawn();

            bullet.gameObject.SetActive(true);
            bullet.Apply(bulletParams);

            bullet.Hit += OnProjectileHit;

            StartLifetime(bullet, bulletParams.LifeSeconds);

            return bullet;
        }

        private void StartLifetime(Bullet bullet, float lifeSeconds)
        {
            int id = bullet.GetInstanceID();

            StopLifetime(id);

            CancellationTokenSource cts = new CancellationTokenSource();
            _lifeTokens.Add(id, cts);

            DespawnAfterDelayAsync(bullet, lifeSeconds, cts.Token).Forget();
        }

        private void StopLifetime(int id)
        {
            if (_lifeTokens.TryGetValue(id, out CancellationTokenSource cts) == false)
            {
                return;
            }

            _lifeTokens.Remove(id);
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

        private void OnProjectileHit(Bullet bullet, Collision collision)
        {
            Despawn(bullet);
        }

        private void Despawn(Bullet bullet)
        {
            int id = bullet.GetInstanceID();
            StopLifetime(id);

            bullet.ResetState();
            bullet.gameObject.SetActive(false);

            _pool.Despawn(bullet);
        }
    }
}
