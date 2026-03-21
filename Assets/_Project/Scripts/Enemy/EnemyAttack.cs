using UnityEngine;
using _Project.Scripts.Logic.Health;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        private EnemyAnimator _animator;
        
        private bool _canAttack = true;
        
        private float _damage;
        private float _attackCooldown;
        private float _attackRange;
        private float _attackTimer;
        private Collider[] _attackTargets;

        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _castleLayer;

        private void Awake()
        {
            _animator = GetComponent<EnemyAnimator>();
            _attackTargets = new Collider[1];
        }

        private void Update()
        {
            if(CanAttack())
                StartAttack();
            
            TickAttackTimer();
        }

        public void Initialize(float damage, float attackCooldown, float attackRange)
        {
            _damage = damage;
            _attackCooldown = attackCooldown;
            _attackRange = attackRange;
        }
        
        private void StartAttack()
        {
            _canAttack = false;
            _attackTimer = _attackCooldown;
            _animator.PlayAttack();
        }
        
        private void OnAttackEnd() =>  _canAttack = true;

        private void OnAttack()
        {
            Vector3 hitPosition = GetHitPosition();
            
            int hits = Physics.OverlapSphereNonAlloc(hitPosition, _attackRadius, _attackTargets, _castleLayer);
            
            if (hits > 0)
                OnHit();
        }

        private void OnHit()
        {
            foreach (Collider target in _attackTargets)
            {
                if (!target.TryGetComponent<IDamagable>(out var damagable))
                    return;
                
                damagable.TakeDamage(_damage);
            } 
        }

        private bool CanAttack() => _canAttack && _attackTimer <= 0;

        private void TickAttackTimer()
        {
            if (_attackTimer > 0)
                _attackTimer -= Time.deltaTime;
        }

        private Vector3 GetHitPosition() => new Vector3(
                                                transform.position.x, 
                                                transform.position.y + _attackRadius / 2, 
                                                transform.position.z)
                                                + transform.forward * _attackRange;
    }
}