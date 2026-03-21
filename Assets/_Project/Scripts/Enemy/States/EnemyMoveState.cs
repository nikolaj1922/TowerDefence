using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy.States
{
    public class EnemyMoveState: IEnterableState, IExitableState
    {
        private readonly EnemyAgentMover _enemyMover;
        private readonly EnemyAnimator _enemyAnimator;
        
        public EnemyMoveState(EnemyAgentMover enemyMover, EnemyAnimator enemyAnimator)
        {
            _enemyMover = enemyMover;
            _enemyAnimator = enemyAnimator;
        }

        public void Enter()
        {
            _enemyMover.enabled = true;
            _enemyAnimator.PlayMove();
        }

        public void Exit() => _enemyMover.enabled = false;
    }
}