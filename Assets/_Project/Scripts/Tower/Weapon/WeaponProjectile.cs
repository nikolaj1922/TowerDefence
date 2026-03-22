using System;
using UnityEngine;
using _Project.Scripts.Enemy;

namespace _Project.Scripts.Tower.Weapon
{
    public class WeaponProjectile: MonoBehaviour
    {
        private event Action OnHit;
        
        [SerializeField] private float _speed;

        private Vector3 _targetPosition;
        private float _damage;
        
        private void Update()
        {
            if (_targetPosition == Vector3.zero)
                return;
            
            transform.position = 
                Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }

        public void Initialize(Vector3 targetPosition, float damage, Action onHit = null)
        {
            _targetPosition = targetPosition;
            _damage = damage;
            OnHit += onHit;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out EnemyController enemy))
                return;
            
            // enemy.TakeDamage(_damage);
            OnHit?.Invoke();
        }
    }
}