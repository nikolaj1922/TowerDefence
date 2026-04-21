using System;
using _Project.Scripts.Enemies.States;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.HealthBar;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _Project.Scripts.Enemies.Behaviour
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
        public float CurrentHealth => HealthModel.CurrentHealth;

        private int _coinReward;
        private Action _onDeath;
        private CoinCounterModel _coinCounter;

        [Inject]
        private void Construct(HealthModel healthModel, CoinCounterModel coinCounterModel)
        {
            _coinCounter = coinCounterModel;
            HealthModel = healthModel;
        }

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

        public void Initialize(Action onDeath, int coinReward)
        {
            _coinReward = coinReward;
            _onDeath = onDeath;
        }
        
        public void SetInitialized() => IsInitialized = true;

        public void ResetComponents()
        {
            HealthModel.Reset();
            
            _col.enabled = true;
            Mover.enabled = false;
            Agent.enabled = true;
            Attack.enabled = false;
        }

        public void TakeDamage(float damage)
        {
            HealthModel.ChangeHealth(-damage);

            if (HealthModel.CurrentHealth > 0)
                return;
            
            _onDeath?.Invoke();
            _coinCounter.AddCoins(_coinReward);
            Death.Die();
        }
        
        public void SetStateMachine(StateMachine stateMachine) => StateMachine = stateMachine;

        public void ToIdle() => StateMachine.SetState(typeof(EnemyIdleState));
    }
}
