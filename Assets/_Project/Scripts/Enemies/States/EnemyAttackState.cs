using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemies.States
{
    public class EnemyAttackState : IEnterableState, IExitableState
    {
        private readonly EnemyAttack _enemyAttack;
        
        public EnemyAttackState(EnemyAttack enemyAttack) => _enemyAttack = enemyAttack;

        public void Enter() => _enemyAttack.enabled = true;

        public void Exit() => _enemyAttack.enabled = false;
    }
}