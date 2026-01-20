using Game.Features.Combat.Domain;
using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity
{
    public sealed class EnemyTeam : MonoBehaviour, ITeamMember
    {
        [SerializeField] private int _teamId = 1;

        public int TeamId => _teamId;
    }
}