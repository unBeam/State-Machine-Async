using UnityEngine;

namespace Game.Features.Combat.Domain
{
    public interface ITargetProvider
    {
        Transform Current { get; }
    }
}