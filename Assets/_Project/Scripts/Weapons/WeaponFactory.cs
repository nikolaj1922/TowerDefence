using _Project.Scripts.Database.Weapons;
using Zenject;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponDatabase _weaponDatabase;
        
        public WeaponFactory(WeaponDatabase weaponDatabase, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _weaponDatabase = weaponDatabase;
        }

        public Weapon CreateWeapon(
            WeaponType type, 
            Vector3 position, 
            Transform parent, 
            float damageMultiplier,
            float attackSpeedMultiplier)

        {
            GameObject weaponObject = _instantiator.InstantiatePrefab(
                _weaponDatabase.Get(type),
                position,
                Quaternion.identity,
                parent);
            
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            weapon.SetMultipliers(damageMultiplier, attackSpeedMultiplier);

            return weapon;
        }
    }
}