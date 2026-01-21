using UnityEngine;

namespace Game.Features.Combat.Domain
{
    public readonly struct BulletShotParams
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Vector3 Velocity { get; }
        public int OwnerTeamId { get; }

        public BulletShotParams(
            Vector3 position,
            Quaternion rotation,
            Vector3 velocity,
            int ownerTeamId)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            OwnerTeamId = ownerTeamId;
        }
    }
}