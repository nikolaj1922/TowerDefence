using System;
using Zenject;
using CartoonFX;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.ObjectsPool;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.Weapons
{
    public class WeaponAttackFX: IInitializable, IDisposable
    {
        private const float VFX_SCALE_MULTIPLIER = 0.2f;

        private readonly float _attackSpeed;
        private readonly Transform _weaponHead;
        private readonly Transform _weaponBase;
        private readonly Transform _projectileSpawnPoint;
        private readonly ObjectsPool<CFXR_Effect> _projectilePool;
        private Vector3 _weaponHeadInitialPosition;
        private CancellationTokenSource _visualCts;

        public WeaponAttackFX(
            WeaponConfig config, 
            [Inject(Id = GameConstants.WEAPON_HEAD_INJECT_ID)] Transform weaponHead,
            [Inject(Id = GameConstants.WEAPON_BASE_INJECT_ID)] Transform weaponBase,
            [Inject(Id = GameConstants.PROJECTILE_POINT_INJECT_ID)] Transform projectileSpawnPoint,
            CFXR_Effect onAttackEffect
        )
        {
            _attackSpeed = config.AttackSpeed;
            _weaponHead = weaponHead;
            _weaponBase = weaponBase;
            _projectileSpawnPoint = projectileSpawnPoint;
            _projectilePool = new ObjectsPool<CFXR_Effect>(onAttackEffect);
        }
        
        public void Initialize()
        {
            _weaponBase.transform.localScale = _weaponBase.transform.parent.localScale;
            _weaponHeadInitialPosition = _weaponHead.position;
        }

        public void PlayRecoil()
        {
            if (!_weaponHead)
                return;

            _weaponHead.position -= _weaponHead.forward * GameConstants.ATTACK_RECOIL;

            CancelVisualTask();
            _visualCts = new CancellationTokenSource();
            BackWeaponHeadToInitialPosition(_visualCts.Token).Forget();
        }
        
        public void CreateAttackFX()
        {
            if (!_projectileSpawnPoint || !_weaponBase)
                return;

            var effect = _projectilePool.Get();
            effect.transform.SetPositionAndRotation(
                _projectileSpawnPoint.position,
                _projectileSpawnPoint.rotation);
            
            effect.transform.localScale = _weaponBase.localScale * VFX_SCALE_MULTIPLIER;

            var vfx = effect.GetComponent<PooledVFX>();
            vfx.Init(_projectilePool, effect);
        }
        
        public void Dispose() => CancelVisualTask();
        
        private async UniTaskVoid BackWeaponHeadToInitialPosition(CancellationToken token)
        {
            if (!_weaponHead)
                return;

            float elapsed = 0f;
            Vector3 startPos = _weaponHead.position;

            while (elapsed < _attackSpeed)
            {
                if (token.IsCancellationRequested || !_weaponHead)
                    return;
                
                _weaponHead.position = Vector3.Lerp(
                    startPos,
                    _weaponHeadInitialPosition,
                    elapsed / _attackSpeed);
                
                elapsed += Time.deltaTime;
                
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
            
            _weaponHead.position = _weaponHeadInitialPosition;
        }
        
        private void CancelVisualTask()
        {
            if (_visualCts == null)
                return;

            _visualCts.Cancel();
            _visualCts.Dispose();
            _visualCts = null;
        }
    }
}