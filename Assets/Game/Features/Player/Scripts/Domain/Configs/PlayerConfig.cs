using Game.Features.Combat.Domain;
using UnityEngine;

namespace Game.Features.Player.Domain
{
    [CreateAssetMenu(menuName = "Game/Player/Player Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxHp { get; private set; } = 100.0f;
        [field: SerializeField] public TeamDefinition Team { get; private set; }
    }
}