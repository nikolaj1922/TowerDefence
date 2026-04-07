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

        [field: SerializeField] public Transform AttackPoint { get; private set; }
        public HealthModel HealthModel { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public EnemyAttack Attack { get; private set; }
        public EnemyAgentMover Mover { get; private set; }
        public EnemyAnimator Animator { get; private set; }
        public EnemyDeath Death { get; private set; }
        public StateMachine StateMachine { private set; get; }
        public bool IsInitialized { get; private set; }
        
        
        [Inject]
        private void Construct(HealthModel healthModel) => HealthModel = healthModel;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Mover = GetComponent<EnemyAgentMover>();
            Death = GetComponent<EnemyDeath>();
            Attack = GetComponent<EnemyAttack>();
            Animator = GetComponent<EnemyAnimator>();

            _col = GetComponent<Collider>();
        }

        private void Update() => StateMachine?.Update();
        
        public void SetInitialized() => IsInitialized = true;

        public void ResetComponents()
        {
            HealthModel.Reset();
            
            _col.enabled = true;
            Mover.enabled = false;
            Agent.enabled = true;
            Attack.enabled = false;
        }

        public void TakeDamage(float damage) => HealthModel.ChangeHealth(-damage);
        
        public void SetStateMachine(StateMachine stateMachine) => StateMachine = stateMachine;
    }
}
