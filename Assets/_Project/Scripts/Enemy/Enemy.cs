using Zenject;
using UnityEngine;
using UnityEngine.AI;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyAttack), typeof(EnemyAgentMover))]
    [RequireComponent(typeof(EnemyAnimator), typeof(EnemyDeath), typeof(Collider))]
    public class Enemy : MonoBehaviour, IDamagable
    {
        private Collider _col;

        public HealthModel HealthModel { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public EnemyAttack Attack { get; private set; }
        public EnemyAgentMover Mover { get; private set; }
        public EnemyAnimator Animator { get; private set; }
        public EnemyDeath Death { get; private set; }

        [field: SerializeField] public Transform AttackPoint { get; private set; }

        private StateMachine _stateMachine;

        [Inject]
        private void Construct(HealthModel healthModel) => HealthModel = healthModel;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Attack = GetComponent<EnemyAttack>();
            Mover = GetComponent<EnemyAgentMover>();
            Animator = GetComponent<EnemyAnimator>();
            Death = GetComponent<EnemyDeath>();
            _col = GetComponent<Collider>();
        }

        private void Update() => _stateMachine?.Update();

        public void ResetComponents()
        {
            _stateMachine = null;
            HealthModel.Reset();
            _col.enabled = true;
            Attack.enabled = false;
            Mover.enabled = false;
            Agent.enabled = true;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"Enemy take damage: {damage}");
            HealthModel.ChangeHealth(-damage);
        }
        
        public void SetStateMachine(StateMachine stateMachine) => _stateMachine = stateMachine;
    }
}
