using Zenject;
using UnityEngine;
using UnityEngine.AI;
using _Project.Scripts.Configs;
using _Project.Scripts.Repositories;

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
        
        public bool IsMoving { get; private set; } = false;
        public bool IsCastleReached { get; private set; } = false;

        [Inject]
        public void Construct(EnemyConfig config, LevelRepository repository)
        {
            _speed = config.speed;
            _attackRange = config.attackRange;
            _destination = repository.LevelConfig.castlePosition;

            IsMoving = true;
        }

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            IsMoving = false;
            IsCastleReached = false;
        }

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _speed;
            _navMeshAgent.destination = _destination;
            _navMeshAgent.stoppingDistance = CASTLE_SIZE + _navMeshAgent.radius + _attackRange;
        }

        private void Update()
        {
            if (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
            
            IsMoving = false;
            IsCastleReached = true;
        }
    }
}