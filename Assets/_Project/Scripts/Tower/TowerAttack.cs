using UnityEngine;

namespace _Project.Scripts.Tower
{
    [RequireComponent(typeof(TowerAim))]
    public class TowerAttack : MonoBehaviour
    {
        private const float MAX_ANGLE_TO_ATTACK = 3f;

        [SerializeField] public Transform _weaponHead;

        private TowerAim _aim;
        private float _damage;
        private float _attackSpeed;
        private float _attackCooldown;

        private void Awake()
        {
            _aim = GetComponent<TowerAim>();
        }

        private void Update()
        {
            if (CanAttack())
                Attack();
            
            Tick();
        }

        public void Initialize(float damage, float attackSpeed)
        {
            _damage = damage;
            _attackSpeed = attackSpeed;
        }

        private void Attack()
        {
            Debug.Log("Attack");
            _attackCooldown = _attackSpeed;
            _aim.Target.TakeDamage(_damage);
        }

        private bool CanAttack()
        {
            if (_aim.Target == null)
                return false;
            
            return _attackCooldown <= 0 && GetAngleToTarget(_aim.Target.AttackPoint.position) <= MAX_ANGLE_TO_ATTACK;
        }

        private float GetAngleToTarget(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - _weaponHead.position).normalized;
            return Vector3.Angle(_weaponHead.forward, directionToTarget);
        }

        private void Tick()
        {
            if(_attackCooldown > 0)
                _attackCooldown -= Time.deltaTime;
        }
    }
}