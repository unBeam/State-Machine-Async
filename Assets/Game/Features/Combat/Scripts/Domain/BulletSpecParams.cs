namespace Game.Features.Combat.Domain
{
    public readonly struct BulletSpecParams
    {
        public float LifeSeconds { get; }
        public float Damage { get; }
        public bool FriendlyFire { get; }

        public BulletSpecParams(float lifeSeconds, float damage, bool friendlyFire)
        {
            LifeSeconds = lifeSeconds;
            Damage = damage;
            FriendlyFire = friendlyFire;
        }
    }
}