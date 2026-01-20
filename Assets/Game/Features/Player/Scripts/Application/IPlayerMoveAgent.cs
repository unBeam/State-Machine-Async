using UnityEngine;

namespace Game.Features.Player.Application
{
    public interface IPlayerMoveAgent
    {
        void SetDestination(Vector3 point);
    }
}