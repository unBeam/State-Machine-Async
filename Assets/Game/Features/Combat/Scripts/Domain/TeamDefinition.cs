using UnityEngine;

namespace Game.Features.Combat.Domain
{
    [CreateAssetMenu(menuName = "Game/Combat/Team Definition")]
    public sealed class TeamDefinition : ScriptableObject
    {
        [field: SerializeField] public int TeamId { get; private set; }
    }
}