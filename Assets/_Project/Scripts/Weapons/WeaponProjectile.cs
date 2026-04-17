using System;
using _Project.Scripts.Enemies.Behaviour;
using _Project.Scripts.Logic.Health;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class WeaponProjectile: MonoBehaviour
    {
        public event Action<WeaponProjectile> OnHit;
        
        [SerializeField] private float _speed;

        private Enemy _target;
        private float _damage;

        private void Update()
        {
            if (_target == null)
            {
                OnHit?.Invoke(this);
                return;
            }
            
            LookAtTarget();
            MoveToTarget();
        }   

        public void Initialize(Enemy target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(_damage);
            
            OnHit?.Invoke(this);
        }
        
        private void LookAtTarget() =>  transform.LookAt(_target.AttackPoint.transform);
        
        private void MoveToTarget() =>         
            transform.position = 
                Vector3.MoveTowards(transform.position, _target.AttackPoint.transform.position, _speed * Time.deltaTime);
    }
}