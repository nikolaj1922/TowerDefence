using System;
using _Project.Scripts.Enemies;
using _Project.Scripts.Logic.Health;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class WeaponProjectile: MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Enemy _target;
        private float _damage;
        private Action _onHit;
        
        private void Update()
        {
            if (_target == null)
            {
                _onHit?.Invoke();
                return;
            }
              

            LookAtTarget();
            MoveToTarget();
        }   

        public void Initialize(Enemy target, float damage, Action onHit = null)
        {
            _target = target;
            _damage = damage;
            _onHit = onHit;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(_damage);
            
            _onHit?.Invoke();
        }
        
        private void LookAtTarget() =>  transform.LookAt(_target.AttackPoint.transform);
        
        private void MoveToTarget() =>         
            transform.position = 
                Vector3.MoveTowards(transform.position, _target.AttackPoint.transform.position, _speed * Time.deltaTime);
    }
}