using _Project.Scripts.Infrastructure.StateMachine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemy.States
{
    public class EnemyDeathState : IEnterableState
    {
        private readonly EnemyAnimator _enemyAnimator;
        private readonly NavMeshAgent _agent;
        
        public EnemyDeathState(EnemyAnimator enemyAnimator, NavMeshAgent agent)
        {
            _enemyAnimator = enemyAnimator;
            _agent = agent;
        }

        public void Enter()
        {
            _agent.enabled = false;
            _enemyAnimator.PlayDeath();
        }
    }
}