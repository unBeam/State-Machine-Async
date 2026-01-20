using Game.Features.Combat.Domain;
using Game.Features.Player.Domain;
using UnityEngine;
using Zenject;

namespace Game.Features.Player.Presentation.Unity
{
    public sealed class PlayerTeam : MonoBehaviour, ITeamMember
    {
        [Inject] private PlayerConfig _config;

        public int TeamId => _config.TeamId;
    }
}