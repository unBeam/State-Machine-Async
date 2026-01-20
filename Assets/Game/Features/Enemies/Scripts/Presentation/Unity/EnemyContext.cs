using Game.Features.Combat.Domain;
using UnityEngine;

namespace Game.Features.Enemies.Presentation.Unity
{
    public sealed class EnemyContext
    {
        private readonly Transform _self;
        private readonly ITargetProvider _targets;
        private readonly int _teamId;

        public EnemyContext(Transform self, ITargetProvider targets, int teamId)
        {
            _self = self;
            _targets = targets;
            _teamId = teamId;
        }

        public Transform Self => _self;
        public Transform Target => _targets.Current;
        public int TeamId => _teamId;
    }
}