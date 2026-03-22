using System;
using _Project.Scripts.Configs;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAgentMover : MonoBehaviour
    {
        private const float CASTLE_SIZE = 0.2f;
        
        private NavMeshAgent _navMeshAgent;

        public bool IsMoving { get; private set; } = false;
        public bool IsCastleReached { get; private set; } = false;

        [Inject]
        public void Construct(EnemyConfig config)
        {
            _navMeshAgent.speed = config.speed;
            _navMeshAgent.stoppingDistance = CASTLE_SIZE + _navMeshAgent.radius + config.attackRange;
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

        private void Update()
        {
            if (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
            
            IsMoving = false;
            IsCastleReached = true;
        }

        public void Initialize(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            IsMoving = true;
        }
    }
}