using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.ObjectsPool;

namespace _Project.Scripts.Weapon
{
    public class WeaponAttack : IInitializable, ITickable
    {
        private const float PROJECTILE_MULTIPLIER = 0.3f;
        
        private readonly Transform _projectileSpawnPoint;
        private readonly Transform _weaponHead;
        private readonly Transform _weaponBase;
        private readonly WeaponProjectile _projectile;
        private readonly WeaponTargetFinder _targetFinder;
        private readonly WeaponAttackFX _weaponAttackFX;
        
        private readonly float _damage;
        private readonly float _attackSpeed;
        private float _attackCooldown;
        private ObjectsPool<WeaponProjectile> _projectilePool;
        
        public WeaponAttack(
            WeaponConfig config, 
            WeaponTargetFinder targetFinder,
            [Inject(Id = GameConstants.WEAPON_HEAD_INJECT_ID)] Transform weaponHead,
            [Inject(Id = GameConstants.WEAPON_BASE_INJECT_ID)] Transform weaponBase,
            [Inject(Id = GameConstants.PROJECTILE_POINT_INJECT_ID)] Transform projectileSpawnPoint,
            WeaponProjectile projectile,
            WeaponAttackFX weaponAttackFX
            )
        {
            _damage = config.damage; 
            _attackSpeed = config.attackSpeed;
            _targetFinder = targetFinder;
            _weaponHead = weaponHead;
            _weaponBase = weaponBase;
            _projectileSpawnPoint = projectileSpawnPoint;
            _projectile = projectile;
            _weaponAttackFX = weaponAttackFX;
        }

        public void Initialize() => _projectilePool = new ObjectsPool<WeaponProjectile>(_projectile);

        public void Tick()
        {
            if (!HasRequiredTransforms())
                return;

            if (CanAttack())
                Attack();
            
            CooldownTick();
        }
        
        private void Attack()
        {
            _weaponAttackFX.PlayRecoil();
            _weaponAttackFX.CreateAttackFX();
            SpawnProjectile();
            _attackCooldown = _attackSpeed;
        }
        
        private void SpawnProjectile()
        {
            if (!HasRequiredTransforms())
                return;

            WeaponProjectile projectile = _projectilePool.Get();

            projectile.transform.localScale = _weaponBase.localScale * PROJECTILE_MULTIPLIER;
            projectile.transform.position = _projectileSpawnPoint.position;
            projectile.Initialize(
                target: _targetFinder.Target,
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
            if (!HasRequiredTransforms() || _targetFinder.Target == null || _targetFinder.Target.AttackPoint == null)
                return false;
            
            return _attackCooldown <= 0 && GetAngleToTarget(_targetFinder.Target.AttackPoint.position) <= GameConstants.MAX_ANGLE_TO_ATTACK;
        }

        private float GetAngleToTarget(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - _weaponHead.position).normalized;
            return Vector3.Angle(_weaponHead.forward, directionToTarget);
        }

        private void CooldownTick()
        {
            if(_attackCooldown > 0)
                _attackCooldown -= Time.deltaTime;
        }

        private bool HasRequiredTransforms() => _weaponHead && _weaponBase && _projectileSpawnPoint;
    }
}
