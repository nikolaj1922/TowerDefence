using Zenject;
using UnityEngine;
using UnityEngine.AI;
using _Project.Scripts.Configs;
using _Project.Scripts.ConfigRepositories;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAgentMover : MonoBehaviour
    {
        private const float CASTLE_SIZE = 0.2f;
        
        private NavMeshAgent _navMeshAgent;
        private float _speed;
        private float _attackRange;
        private Vector3 _destination;
        
        public bool IsTargetReached { get; private set; }

        [Inject]
        public void Construct(EnemyConfig config, GameRepository repository)
        {
            _speed = config.speed;
            _attackRange = config.attackRange;
            _destination = repository.GameConfig.castlePosition;
        }

        private void Awake() => _navMeshAgent = GetComponent<NavMeshAgent>();

        private void OnEnable()
        {
            if (!_navMeshAgent.isOnNavMesh)
                return;

            _navMeshAgent.ResetPath();
            _navMeshAgent.SetDestination(_destination);
            
           
            IsTargetReached = false;
            _navMeshAgent.speed = _speed;
            _navMeshAgent.stoppingDistance = CASTLE_SIZE + _navMeshAgent.radius + _attackRange;
        }

        private void Update()
        {
            if (_navMeshAgent.pathPending || IsTargetNotReached()) return;
            
            IsTargetReached = true;
        }

        private bool IsTargetNotReached() => _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;
    }
}
