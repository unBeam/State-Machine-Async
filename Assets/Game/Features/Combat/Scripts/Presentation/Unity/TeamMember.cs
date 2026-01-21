using Game.Features.Combat.Domain;
using UnityEngine;

namespace Game.Features.Combat.Presentation.Unity
{
    public sealed class TeamMember : MonoBehaviour, ITeamMember
    {
        [SerializeField] private TeamDefinition _team;

        public int TeamId
        {
            get
            {
                if (_team == null)
                {
                    return 0;
                }

                return _team.TeamId;
            }
        }
    }
}