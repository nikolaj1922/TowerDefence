using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Health;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        private const int MAX_TARGETS = 1;
        private const float ATTACK_HEIGHT_OFFSET_MULTIPLIER = 0.5f;
        
        private EnemyAnimator _animator;
        private bool _canAttack = true;
        private float _damage;
        private float _attackCooldown;
        private float _attackRange;
        private float _attackTimer;
        private Collider[] _attackTargets;

        [SerializeField] private float _attackRadius;
        [SerializeField] private LayerMask _castleLayer;
        
        [Inject]
        public void Construct(EnemyConfig config)
        {
            _damage = config.damage;
            _attackCooldown = config.attackCooldown;
            _attackRange = config.attackRange;
        }

        private void Awake()
        {
            _animator = GetComponent<EnemyAnimator>();
            _attackTargets = new Collider[MAX_TARGETS];
        }

        private void OnEnable()
        {
            _canAttack = true;
            _attackTimer = 0f;
        }
        
        private void Update()
        {
            if(CanAttack())
                StartAttack();
            
            TickAttackTimer();
        }
        
        private void StartAttack()
        {
            _canAttack = false;
            _attackTimer = _attackCooldown;
            _animator.PlayAttack();
        }
        
        private void OnAttackEnd() => _canAttack = true;

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
                                                transform.position.y + _attackRadius * ATTACK_HEIGHT_OFFSET_MULTIPLIER, 
                                                transform.position.z)
                                                + transform.forward * _attackRange;
    }
}