using _Project.Scripts.Infrastructure.StateMachine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.States
{
    public class EnemyMoveState: IEnterableState, IExitableState
    {
        private readonly NavMeshAgent _agent;
        private readonly EnemyAgentMover _enemyMover;
        private readonly EnemyAnimator _enemyAnimator;
        
        public EnemyMoveState(EnemyAgentMover enemyMover, NavMeshAgent agent, EnemyAnimator enemyAnimator)
        {
            _enemyMover = enemyMover;
            _enemyAnimator = enemyAnimator;
            _agent = agent;
        }

        public void Enter()
        {
            _agent.enabled = true;
            _enemyMover.enabled = true;
            _enemyAnimator.PlayMove();
        }

        public void Exit()
        {
            _agent.enabled = false;
            _enemyMover.enabled = false;
        }
    }
}