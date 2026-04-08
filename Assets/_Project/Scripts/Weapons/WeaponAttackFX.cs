using System;
using System.Threading;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.GameConstants;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Weapons
{
    public class WeaponAttackFX: IInitializable, IDisposable
    {
        private const float VFX_SCALE_MULTIPLIER = 0.2f;

        private readonly float _attackSpeed;
        private readonly Transform _weaponHead;
        private readonly Transform _weaponBase;
        private readonly Transform _projectileSpawnPoint;
        private readonly ParticleSystem _onAttackEffect;
        private Vector3 _weaponHeadInitialPosition;
        private CancellationTokenSource _visualCts;

        public WeaponAttackFX(
            WeaponConfig config, 
            [Inject(Id = GameConstants.WEAPON_HEAD_INJECT_ID)] Transform weaponHead,
            [Inject(Id = GameConstants.WEAPON_BASE_INJECT_ID)] Transform weaponBase,
            [Inject(Id = GameConstants.PROJECTILE_POINT_INJECT_ID)] Transform projectileSpawnPoint,
            ParticleSystem onAttackEffect
        )
        {
            _attackSpeed = config.AttackSpeed;
            _weaponHead = weaponHead;
            _weaponBase = weaponBase;
            _projectileSpawnPoint = projectileSpawnPoint;
            _onAttackEffect = onAttackEffect;
        }
        
        public void Initialize() => _weaponHeadInitialPosition = _weaponHead.position;
        
        public void PlayRecoil()
        {
            if (!_weaponHead)
                return;

            _weaponHead.position -= _weaponHead.forward * GameConstants.ATTACK_RECOIL;

            CancelVisualTask();
            _visualCts = new CancellationTokenSource();
            BackVisualToInitialHeadPosition(_visualCts.Token).Forget();
        }
        
        public void CreateAttackFX()
        {
            if (!_projectileSpawnPoint || !_weaponBase)
                return;

            var effect = Object.Instantiate(_onAttackEffect, _projectileSpawnPoint.position, Quaternion.identity);
            effect.transform.localScale = _weaponBase.localScale * VFX_SCALE_MULTIPLIER;
        }
        
        public void Dispose() => CancelVisualTask();
        
        private async UniTaskVoid BackVisualToInitialHeadPosition(CancellationToken token)
        {
            if (!_weaponHead)
                return;

            try
            {
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

                if (!_weaponHead)
                    return;

                _weaponHead.position = _weaponHeadInitialPosition;
            }
            catch (OperationCanceledException)
            {
                return;
            }
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