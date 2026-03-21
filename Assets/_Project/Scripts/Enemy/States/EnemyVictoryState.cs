using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy.States
{
    public class EnemyVictoryState: IEnterableState
    {
        private readonly EnemyAgentMover _enemyAgentMover;
        private readonly EnemyAttack _enemyAttack;
        private readonly EnemyAnimator _enemyAnimator;
        
        public EnemyVictoryState(EnemyAgentMover enemyAgentMover, EnemyAttack enemyAttack, EnemyAnimator enemyAnimator)
        {
            _enemyAgentMover = enemyAgentMover;
            _enemyAnimator = enemyAnimator;
            _enemyAttack = enemyAttack;
        }

        public void Enter()
        {
            _enemyAgentMover.enabled = false;
            _enemyAttack.enabled = false;
            _enemyAnimator.PlayVictory();
        }
    }
}