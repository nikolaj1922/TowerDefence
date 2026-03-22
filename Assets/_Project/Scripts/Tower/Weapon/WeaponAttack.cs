using UnityEngine;
using System.Collections;
using _Project.Scripts.Infrastructure.ObjectsPool;

namespace _Project.Scripts.Tower.Weapon
{
    [RequireComponent(typeof(WeaponTargetFinder))]
    public class WeaponAttack : MonoBehaviour
    {
        private const float MAX_ANGLE_TO_ATTACK = 3f;
        private const float ATTACK_RECOIL = 0.03f;

        [SerializeField] public Transform _projectileSpawnPoint;
        [SerializeField] public Transform _weaponHead;
        [SerializeField] public WeaponProjectile _projectile;

        private WeaponTargetFinder _targetFinder;
        private float _damage;
        private float _attackSpeed;
        private float _attackCooldown;
        private Vector3 _weaponHeadInitialPosition;
        private ObjectsPool<WeaponProjectile> _projectilePool;

        private Coroutine _visualRoutine;

        private void Awake()
        {
            _targetFinder = GetComponent<WeaponTargetFinder>();
        }

        private void Start()
        {
            _weaponHeadInitialPosition = _weaponHead.position;
            _projectilePool = new ObjectsPool<WeaponProjectile>(_projectile);
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
            MoveHead();
            SpawnProjectile();
            _attackCooldown = _attackSpeed;
        }

        private void SpawnProjectile()
        {
            WeaponProjectile projectile = _projectilePool.Get();
            
            projectile.transform.position = _projectileSpawnPoint.position;
            projectile.Initialize(
                targetPosition: _targetFinder.Target.transform.position,
                damage: _damage,
                onHit: () => OnProjectileHit(projectile));
        }

        private void OnProjectileHit(WeaponProjectile projectile)
        {
            if (_targetFinder?.Target?.HealthModel.CurrentHealth <= 0)
                _targetFinder.ResetTarget();
            
            _projectilePool.Release(projectile);
        }

        private bool CanAttack()
        {
            if (_targetFinder.Target == null)
                return false;
            
            return _attackCooldown <= 0 && GetAngleToTarget(_targetFinder.Target.AttackPoint.position) <= MAX_ANGLE_TO_ATTACK;
        }

        private float GetAngleToTarget(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - _weaponHead.position).normalized;
            return Vector3.Angle(_weaponHead.forward, directionToTarget);
        }

        private void MoveHead()
        {
            _weaponHead.position -= _weaponHead.forward * ATTACK_RECOIL;

            if (_visualRoutine != null)
                StopCoroutine(_visualRoutine);

            _visualRoutine = StartCoroutine(BackVisualToInitialHeadPosition());
        }

        private IEnumerator BackVisualToInitialHeadPosition()
        {
            float elapsed = 0f;
            Vector3 startPos = _weaponHead.position;

            while (elapsed < _attackSpeed)
            {
                _weaponHead.position = Vector3.Lerp(startPos, _weaponHeadInitialPosition, elapsed / _attackSpeed);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _weaponHead.position = _weaponHeadInitialPosition;
            _visualRoutine = null;
        }

        private void Tick()
        {
            if(_attackCooldown > 0)
                _attackCooldown -= Time.deltaTime;
        }
    }
}