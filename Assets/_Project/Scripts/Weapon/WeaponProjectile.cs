using System;
using UnityEngine;
using _Project.Scripts.Logic.Health;

namespace _Project.Scripts.Weapon
{
    public class WeaponProjectile: MonoBehaviour
    {
        private event Action OnHit;
        
        [SerializeField] private float _speed;

        private Enemy.Enemy _target;
        private float _damage;
        
        private void Update()
        {
            if (_target == null)
                return;

            LookAtTarget();
            MoveToTarget();
        }   

        public void Initialize(Enemy.Enemy target, float damage, Action onHit = null)
        {
            _target = target;
            _damage = damage;
            OnHit += onHit;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IDamagable damagable))
            {
                Destroy(gameObject);
                return;
            }
            
            damagable.TakeDamage(_damage);
            OnHit?.Invoke();
        }
        
        private void LookAtTarget() =>  transform.LookAt(_target.AttackPoint.transform);
        
        private void MoveToTarget() =>         
            transform.position = 
                Vector3.MoveTowards(transform.position, _target.AttackPoint.transform.position, _speed * Time.deltaTime);
    }
}