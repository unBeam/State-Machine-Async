namespace Game.Features.Combat.Domain
{
    public readonly struct BulletSpawnParams
    {
        public BulletShotParams Shot { get; }
        public BulletSpecParams Spec { get; }

        public BulletSpawnParams(BulletShotParams shot, BulletSpecParams spec)
        {
            Shot = shot;
            Spec = spec;
        }
    }
}