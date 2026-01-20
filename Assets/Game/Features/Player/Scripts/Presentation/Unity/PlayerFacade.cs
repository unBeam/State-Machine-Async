using Game.Features.Combat.Application;
using Game.Features.Combat.Domain;
using Game.Features.Player.Application;
using Game.Features.Player.Domain;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Features.Player.Presentation.Unity
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class PlayerFacade : MonoBehaviour, IDamageable
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _navMeshSampleRadius = 1.0f;

        private PlayerMoveService _move;

        [Inject] private PlayerConfig _config;
        [Inject] private PlayerRegistry _registry;
        [Inject] private PlayerHealth _health;

        private void Awake()
        {
            _registry.Register(transform);

            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            _move = new PlayerMoveService(new NavMeshMoveAgentAdapter(agent));
        }

        private void OnDestroy()
        {
            _registry.Unregister(transform);
        }

        private void Update()
        {
            Mouse mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            if (mouse.leftButton.wasPressedThisFrame == false)
            {
                return;
            }

            Vector2 screenPos = mouse.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(screenPos);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500.0f, _groundMask, QueryTriggerInteraction.Ignore) == false)
            {
                return;
            }

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, _navMeshSampleRadius, NavMesh.AllAreas) == false)
            {
                return;
            }

            _move.MoveTo(navHit.position);
        }

        public void ApplyDamage(float value)
        {
            _health.ApplyDamage(value);
        }
    }
}
