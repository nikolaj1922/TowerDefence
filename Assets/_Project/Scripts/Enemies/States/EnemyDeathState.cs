using _Project.Scripts.Enemies.Behaviour;
using _Project.Scripts.Infrastructure.StateMachine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States
{
    public class EnemyDeathState : IEnterableState
    {
        private readonly NavMeshAgent _agent;
        private readonly EnemyAnimator _enemyAnimator;

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