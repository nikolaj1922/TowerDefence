using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Logic.Health;

namespace _Project.Scripts.Weapon
{
    public class WeaponTargetFinder : ITickable
    {
        private const int MAX_TARGETS = 10;
        
        private readonly Vector3 _position;
        private readonly float _attackRange;
        private readonly LayerMask _enemiesLayerMask;
        private readonly Collider[] _aimedEnemyColliders = new Collider[MAX_TARGETS];
        private readonly HealthModel _castleHealthModel;

        public Enemy.Enemy Target { get; private set; }

        public WeaponTargetFinder(
            [Inject(Id = GameConstants.CASTLE_HEALTH_MODEL_INJECT_ID)] HealthModel healthModel,
            LayerMask enemiesLayerMask, 
            WeaponConfig config, 
            Vector3 position)
        {
            _castleHealthModel = healthModel;
            _enemiesLayerMask = enemiesLayerMask;
            _attackRange = config.attackRange;
            _position = position;
        }

        public void ResetTarget() => Target = null;
        
        public void Tick()
        {
            if (_castleHealthModel.CurrentHealth <= 0)
            {
                Target = null;
                return;
            }

            if (!IsTargetValid())
                Target = null;
            
            if (Target == null)
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

        private Enemy.Enemy GetClosestEnemy(int count)
        {
            Enemy.Enemy closest = null;
            float closestDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var col = _aimedEnemyColliders[i];

                if (!col.TryGetComponent(out Enemy.Enemy enemy))
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

        private bool IsTargetValid() =>
            Target != null
            && Target.HealthModel.CurrentHealth > 0
            && IsTargetInRange();
        
        private bool IsTargetInRange() => (Target.transform.position - _position).sqrMagnitude <= _attackRange * _attackRange;
    }
}