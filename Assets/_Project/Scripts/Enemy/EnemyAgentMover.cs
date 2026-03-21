using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAgentMover : MonoBehaviour
    {
        private const float CASTLE_SIZE = 0.2f;
        
        private NavMeshAgent _navMeshAgent;

        public bool IsMoving { get; private set; } = false;
        public bool IsCastleReached { get; private set; } = false;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
            
            IsMoving = false;
            IsCastleReached = true;
        }

        public void Initialize(float speed, float attackRange, Vector3 destination)
        {
            _navMeshAgent.speed = speed;
            _navMeshAgent.stoppingDistance = CASTLE_SIZE + _navMeshAgent.radius + attackRange;
            _navMeshAgent.destination = destination;
            
            IsMoving = true;
        }
    }
}