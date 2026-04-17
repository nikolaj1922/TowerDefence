using Zenject;
using UnityEngine;
using _Project.Scripts.Database.WeaponPrefabDatabase;

namespace _Project.Scripts.Weapons
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponPrefabsDatabase _weaponPrefabsDatabase;
        
        public WeaponFactory(WeaponPrefabsDatabase weaponPrefabsDatabase, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _weaponPrefabsDatabase = weaponPrefabsDatabase;
        }

        public Weapon CreateWeapon(
            WeaponType type, 
            Vector3 position, 
            Transform parent, 
            float damageMultiplier,
            float attackSpeedMultiplier)

        {
            GameObject weaponObject = _instantiator.InstantiatePrefab(
                _weaponPrefabsDatabase.Get(type),
                position,
                Quaternion.identity,
                parent);
            
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            weapon.SetMultipliers(damageMultiplier, attackSpeedMultiplier);

            return weapon;
        }
    }
}