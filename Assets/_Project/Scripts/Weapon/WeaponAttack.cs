using Zenject;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.ObjectsPool;

namespace _Project.Scripts.Weapon
{
    public class WeaponAttack : IInitializable, ITickable
    {
        private const float PROJECTILE_MULTIPLIER = 0.3f;
        private const float VFX_MULTIPLIER = 0.2f;
        
        private readonly Transform _projectileSpawnPoint;
        private readonly Transform _weaponHead;
        private readonly Transform _weaponBase;
        private readonly WeaponProjectile _projectile;
        private readonly WeaponTargetFinder _targetFinder;
        private readonly ParticleSystem _onAttackEffect;
        private readonly ParticleSystem _onParticleHitEffect;
        
        private readonly float _damage;
        private readonly float _attackSpeed;
        private float _attackCooldown;
        private Vector3 _weaponHeadInitialPosition;
        private ObjectsPool<WeaponProjectile> _projectilePool;
        
        private CancellationTokenSource _visualCts;
        
        public WeaponAttack(
            WeaponConfig config, 
            WeaponTargetFinder targetFinder,
            [Inject(Id = GameConstants.WEAPON_HEAD_INJECT_ID)] Transform weaponHead,
            [Inject(Id = GameConstants.WEAPON_BASE_INJECT_ID)] Transform weaponBase,
            [Inject(Id = GameConstants.PROJECTILE_POINT_INJECT_ID)] Transform projectileSpawnPoint,
            WeaponProjectile projectile,
            ParticleSystem onAttackEffect
            )
        {
            _damage = config.damage; 
            _attackSpeed = config.attackSpeed;
            _targetFinder = targetFinder;
            _weaponHead = weaponHead;
            _weaponBase = weaponBase;
            _projectileSpawnPoint = projectileSpawnPoint;
            _projectile = projectile;
            _onAttackEffect = onAttackEffect;
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

            projectile.transform.localScale = _weaponBase.localScale * PROJECTILE_MULTIPLIER;
            projectile.transform.position = _projectileSpawnPoint.position;
            projectile.Initialize(
                target: _targetFinder.Target,
                damage: _damage,
                onHit: () => OnProjectileHit(projectile));
            AttackFX();
        }

        private void AttackFX()
        {
            var effect = Object.Instantiate(_onAttackEffect, _projectileSpawnPoint.position, Quaternion.identity);
            effect.transform.localScale = _weaponBase.localScale * VFX_MULTIPLIER;
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
            
            return _attackCooldown <= 0 && GetAngleToTarget(_targetFinder.Target.AttackPoint.position) <= GameConstants.MAX_ANGLE_TO_ATTACK;
        }

        private float GetAngleToTarget(Vector3 targetPosition)
        {
            Vector3 directionToTarget = (targetPosition - _weaponHead.position).normalized;
            return Vector3.Angle(_weaponHead.forward, directionToTarget);
        }

        private void MoveHead()
        {
            _weaponHead.position -= _weaponHead.forward * GameConstants.ATTACK_RECOIL;

            _visualCts?.Cancel();
            _visualCts?.Dispose();
            _visualCts = new CancellationTokenSource();
            BackVisualToInitialHeadPosition(_visualCts.Token).Forget();
        }

        private async UniTaskVoid BackVisualToInitialHeadPosition(CancellationToken token)
        {
            float elapsed = 0f;
            Vector3 startPos = _weaponHead.position;

            while (elapsed < _attackSpeed)
            {
                if (token.IsCancellationRequested)
                    return;
                
                _weaponHead.position = Vector3.Lerp(
                    startPos, 
                    _weaponHeadInitialPosition, 
                    elapsed / _attackSpeed);
                
                elapsed += Time.deltaTime;
                
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            _weaponHead.position = _weaponHeadInitialPosition;
        }

        private void CooldownTick()
        {
            if(_attackCooldown > 0)
                _attackCooldown -= Time.deltaTime;
        }
    }
}
