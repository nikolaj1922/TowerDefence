using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy.States
{
    public class EnemyDeathState : IEnterableState
    {
        private readonly EnemyAnimator _enemyAnimator;
        
        public EnemyDeathState(EnemyAnimator enemyAnimator)
        {
            _enemyAnimator = enemyAnimator;
        }

        public void Enter() => _enemyAnimator.PlayDeath();
    }
}