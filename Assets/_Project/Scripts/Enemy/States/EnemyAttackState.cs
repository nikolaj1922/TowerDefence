using _Project.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace _Project.Scripts.Enemy.States
{
    public class EnemyAttackState : IEnterableState, IExitableState
    {
        private readonly EnemyAttack _enemyAttack;
        
        public EnemyAttackState(EnemyAttack enemyAttack) => _enemyAttack = enemyAttack;

        public void Enter() => _enemyAttack.enabled = true;

        public void Exit() => _enemyAttack.enabled = false;
    }
}