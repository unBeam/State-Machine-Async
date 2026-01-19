using UnityEngine;

public struct BulletSpawnParams
{
    public readonly Vector3 Position;
    public readonly Quaternion Rotation;
    public readonly Vector3 Velocity;
    public readonly float LifeSeconds;

    public BulletSpawnParams(Vector3 position, Quaternion rotation, Vector3 velocity, float lifeSeconds)
    {
        Position = position;
        Rotation = rotation;
        Velocity = velocity;
        LifeSeconds = lifeSeconds;
    }
}