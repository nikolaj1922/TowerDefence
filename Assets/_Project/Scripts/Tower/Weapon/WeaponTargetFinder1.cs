using _Project.Scripts.Configs;
using _Project.Scripts.Enemy;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Tower.Weapon
{
    public class WeaponTargetFinder1 : ITickable
    {
        private readonly LayerMask _enemiesLayerMask;
        private readonly Collider[] _aimedEnemyColliders = new Collider[10];
        private readonly Vector3 _position;
        private readonly float _attackRange;
        
        public EnemyController Target { get; private set; }

        public WeaponTargetFinder1(LayerMask enemiesLayerMask, TowerConfig config, Vector3 position)
        {
            _enemiesLayerMask = enemiesLayerMask;
            _attackRange = config.attackRange;
            _position = position;
        }

        public void ResetTarget() => Target = null;
        
        public void Tick()
        {
            if (Target == null || !IsTargetValid())
                SearchForTarget();
        }

        private void SearchForTarget()
        {
            int overlappedEnemyCount = Physics.OverlapSphereNonAlloc(
                _position, _attackRange, _aimedEnemyColliders, _enemiesLayerMask);

            if (overlappedEnemyCount == 0)
                return;

            Target = GetClosestEnemy(overlappedEnemyCount);
        }

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

                float dist = (enemy.transform.position - _position).sqrMagnitude;

                if (dist < closestDist)
                {
                    closest = enemy;
                    closestDist = dist;
                }
            }

            return closest;
        }

        private bool IsTargetValid()
        {
            return Target != null && Target.HealthModel.CurrentHealth > 0;
        }
    }
}