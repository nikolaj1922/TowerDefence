using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Weapon
{
    public class WeaponAim
    {
        private readonly Transform _baseTransform;
        private readonly Transform _weaponHead;
        private readonly WeaponTargetFinder _targetFinder;
        private readonly float _rotationSpeed;

        public WeaponAim(
            [Inject(Id = "WeaponBase")] Transform baseTransform,
            [Inject(Id = "WeaponHead")] Transform weaponHead,
            WeaponTargetFinder targetFinder,
            WeaponConfig config)
        {
            _baseTransform = baseTransform;
            _weaponHead = weaponHead;
            _targetFinder = targetFinder;
            _rotationSpeed = config.rotationSpeed;
        }

        public void Tick(float deltaTime)
        {
            if (_targetFinder.Target == null)
                return;

            LookToTarget(_targetFinder.Target.AttackPoint, deltaTime);
        }

        private void LookToTarget(Transform target, float  deltaTime)
        {
            RotateBase(target.position, deltaTime);

            if (_weaponHead != null)
                RotateWeaponHead(target.position, deltaTime);
        }

        private void RotateWeaponHead(Vector3 targetPosition, float deltaTime)
        {
            Vector3 localDirection = _baseTransform.InverseTransformPoint(targetPosition) -
                                     _baseTransform.InverseTransformPoint(_weaponHead.position);

            float pitch = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            Quaternion targetLocalRotation = Quaternion.Euler(-pitch, 0f, 0f);

            _weaponHead.localRotation = Quaternion.Slerp(
                _weaponHead.localRotation,
                targetLocalRotation,
                deltaTime * _rotationSpeed);
        }

        private void RotateBase(Vector3 targetPosition, float deltaTime)
        {
            Vector3 baseDirection = targetPosition - _baseTransform.position;
            baseDirection.y = 0f;

            if (baseDirection.sqrMagnitude > 0.001f)
                _baseTransform.rotation = Quaternion.Slerp(
                    _baseTransform.rotation,
                    Quaternion.LookRotation(baseDirection),
                    deltaTime * _rotationSpeed);
        }
    }
}