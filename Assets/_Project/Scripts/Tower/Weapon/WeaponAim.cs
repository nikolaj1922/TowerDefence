using UnityEngine;

namespace _Project.Scripts.Tower.Weapon
{
    [RequireComponent(typeof(WeaponTargetFinder))]
    public class WeaponAim : MonoBehaviour
     {
        [SerializeField] private Transform _weaponHead;

        private WeaponTargetFinder _targetFinder;
        private float _rotationSpeed;

        private void Awake()
        {
            _targetFinder = GetComponent<WeaponTargetFinder>();
        }

        private void Update()
        {
            if (_targetFinder.Target == null)
                return;
            
            LookToTarget(_targetFinder.Target.AttackPoint);
        }

        public void Initialize(float rotationSpeed) => _rotationSpeed = rotationSpeed;

        private void LookToTarget(Transform target)
        {
            RotateBase(target.position);

            if (_weaponHead == null)
                return;

            RotateWeaponHead(target.position);
        }
        
        private void RotateWeaponHead(Vector3 targetPosition)
        {
            Vector3 localDirection = transform.InverseTransformPoint(targetPosition) - 
                                     transform.InverseTransformPoint(_weaponHead.position);

            float pitch = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            Quaternion targetLocalRotation = Quaternion.Euler(-pitch, 0f, 0f);

            _weaponHead.localRotation = Quaternion.Slerp(
                _weaponHead.localRotation,
                targetLocalRotation,
                Time.deltaTime * _rotationSpeed);
        }

        private void RotateBase(Vector3 targetPosition)
        {
            Vector3 baseDirection = targetPosition - transform.position;
            baseDirection.y = 0f;

            if (baseDirection.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation(baseDirection), 
                    Time.deltaTime * _rotationSpeed);
        }
    }
}