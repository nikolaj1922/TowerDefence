using UnityEngine;
using _Project.Scripts.Enemy;

namespace _Project.Scripts.Tower.Weapon
{
    public class WeaponTargetFinder : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemiesLayerMask;
        
        private float _attackRange;
        private readonly Collider[] _aimedEnemyColliders = new Collider[10];
        
        public EnemyController Target { get; private set; }
        
        private void Update()
        {
            if (Target == null || !IsTargetValid())
                SearchForTarget();
        }
        
        public void Initialize(float attackRange) => _attackRange = attackRange;

        public void ResetTarget() => Target = null;
        
        private void SearchForTarget()
        {
            int overlappedEnemyCount = GetOverlappedEnemyCount();


            if (overlappedEnemyCount == 0)
                return;
            
            Target = GetClosestEnemy(overlappedEnemyCount);
        }

        private int GetOverlappedEnemyCount() => Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _aimedEnemyColliders, _enemiesLayerMask);

        private EnemyController GetClosestEnemy(int count)
        {
            EnemyController closest = null;
            float closestDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var col = _aimedEnemyColliders[i];

                if (!col.TryGetComponent(out EnemyController enemy))
                    continue;

                if (enemy.HealthModel.CurrentHealth <= 0)
                    continue;

                float dist = (enemy.transform.position - transform.position).sqrMagnitude;

                if (dist < closestDist)
                {
                    closest = enemy;
                    closestDist = dist;
                }
            }

            return closest;
        }
        
        private bool IsTargetValid() => Target != null && Target.HealthModel.CurrentHealth > 0;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}