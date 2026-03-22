using System;
using UnityEngine;

namespace _Project.Scripts.Tower.Weapon
{
    [RequireComponent(typeof(WeaponAim), typeof(WeaponAttack), typeof(WeaponTargetFinder))]
    public class Weapon: MonoBehaviour
    {
        private WeaponAim _aim;
        private WeaponAttack _attack;
        private WeaponTargetFinder _targetFinder;

        private void Awake()
        {
            _aim = GetComponent<WeaponAim>();
            _attack = GetComponent<WeaponAttack>();
            _targetFinder = GetComponent<WeaponTargetFinder>();
        }

        public void Initialize(float attackRange, float damage, float attackSpeed, float rotationSpeed)
        {
            _targetFinder.Initialize(attackRange);
            _attack.Initialize(damage, attackSpeed);
            _aim.Initialize(rotationSpeed);
        }
    }
}