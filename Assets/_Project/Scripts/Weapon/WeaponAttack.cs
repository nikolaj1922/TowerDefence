using Zenject;
using UnityEngine;
using System.Collections;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.CoroutineRunner;
using _Project.Scripts.Infrastructure.ObjectsPool;

namespace _Project.Scripts.Weapon
{
    public class WeaponAttack : IInitializable, ITickable
    {
        private const float MAX_ANGLE_TO_ATTACK = 3f;
        private const float ATTACK_RECOIL = 0.03f;

        private readonly Transform _projectileSpawnPoint;
        private readonly Transform _weaponHead;
        private readonly WeaponProjectile _projectile;
        private readonly WeaponTargetFinder _targetFinder;
        
        private readonly float _damage;
        private readonly float _attackSpeed;
        private float _attackCooldown;
        private Vector3 _weaponHeadInitialPosition;
        private ObjectsPool<WeaponProjectile> _projectilePool;

        private Coroutine _visualRoutine;
        private readonly CoroutineRunner _coroutineRunner;

        public WeaponAttack(
            CoroutineRunner coroutineRunner, 
            WeaponConfig config, 
            WeaponTargetFinder targetFinder,
            [Inject(Id = "WeaponHead")] Transform weaponHead,
            [Inject(Id = "ProjectileSpawnPoint")] Transform projectileSpawnPoint,
            WeaponProjectile projectile
            )
        {
            _coroutineRunner = coroutineRunner;
            _damage = config.damage; 
            _attackSpeed = config.attackSpeed;
            _targetFinder = targetFinder;
            _weaponHead = weaponHead;
            _projectileSpawnPoint = projectileSpawnPoint;
            _projectile = projectile;
        }

        public void Initialize()
        {
            _weaponHeadInitialPosition = _weaponHead.position;
            _projectilePool = new ObjectsPool<WeaponProjectile>(_projectile);
        }

        public void Tick()
        {
            if (CanAttack())
                Attack();
            
            CooldownTick();
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
            if (_targetFinder.Target?.HealthModel.CurrentHealth <= 0)
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
                _coroutineRunner.Stop(_visualRoutine);

            _visualRoutine = _coroutineRunner.Run(BackVisualToInitialHeadPosition());
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

        private void CooldownTick()
        {
            if(_attackCooldown > 0)
                _attackCooldown -= Time.deltaTime;
        }
    }
}
