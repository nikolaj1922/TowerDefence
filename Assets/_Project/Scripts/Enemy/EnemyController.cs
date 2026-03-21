using _Project.Scripts.Logic.Health;
using _Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace _Project.Scripts.Enemy
{
    public class EnemyController : HealthController
    {
        [field: SerializeField] public Transform AttackPoint { get; private set; }

        private StateMachine _stateMachine;

        private void Update() => _stateMachine?.Update();
        
        public void SetStateMachine(StateMachine stateMachine) => _stateMachine = stateMachine;
    }
}