using _Project.Scripts.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Tower
{
    public class TowerAim : MonoBehaviour
    {
        [SerializeField] public Transform _weaponHead;
        [SerializeField] public Transform _weaponBase;
        [SerializeField] private LayerMask _enemiesLayerMask;

        private float _attackRange;
        private float _rotationSpeed;
        private readonly Collider[] _aimedEnemyColliders = new Collider[10];
        
        public EnemyController Target { get; private set; }

        private void Update() => SearchForTarget();

        public void Initialize(float aimRadius, float rotationSpeed)
        {
            _attackRange = aimRadius;
            _rotationSpeed = rotationSpeed;
        }

        private void SearchForTarget()
        {
            int overlappedEnemyCount = GetOverlappedEnemyCount();

            if (overlappedEnemyCount == 0)
                return;

            Transform closestEnemy = GetClosestEnemy(overlappedEnemyCount);

            if (closestEnemy == null)
                return;

            Target = closestEnemy.GetComponent<EnemyController>();
            LookToTarget(Target.AttackPoint);
        }

        private int GetOverlappedEnemyCount() => Physics.OverlapSphereNonAlloc(transform.position, _attackRange, _aimedEnemyColliders, _enemiesLayerMask);

        private Transform GetClosestEnemy(int overlappedEnemyCount)
        {
            NavMeshAgent targetAgent = null;

            for (int i = 0; i < overlappedEnemyCount; i++)
            {
                Collider col = _aimedEnemyColliders[i];

                if (col == null)
                    continue;

                if (!col.TryGetComponent(out NavMeshAgent enemyAgent))
                    continue;

                if (targetAgent == null || enemyAgent.remainingDistance < targetAgent.remainingDistance)
                {
                    targetAgent = enemyAgent;
                }
            }

            return targetAgent?.transform;
        }

        private void LookToTarget(Transform target)
        {
            RotateBase(target.position);

            if (_weaponHead == null)
                return;

            RotateWeaponHead(target.position);
        }
        
        private void RotateWeaponHead(Vector3 targetPosition)
        {
            Vector3 localDirection = _weaponBase.InverseTransformPoint(targetPosition) - 
                                     _weaponBase.InverseTransformPoint(_weaponHead.position);

            float pitch = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            Quaternion targetLocalRotation = Quaternion.Euler(-pitch, 0f, 0f);

            _weaponHead.localRotation = Quaternion.Slerp(
                _weaponHead.localRotation,
                targetLocalRotation,
                Time.deltaTime * _rotationSpeed);
        }

        private void RotateBase(Vector3 targetPosition)
        {
            Vector3 baseDirection = targetPosition - _weaponBase.position;
            baseDirection.y = 0f;

            if (baseDirection.sqrMagnitude > 0.001f)
                _weaponBase.rotation = Quaternion.Slerp(
                    _weaponBase.rotation, 
                    Quaternion.LookRotation(baseDirection), 
                    Time.deltaTime * _rotationSpeed);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
