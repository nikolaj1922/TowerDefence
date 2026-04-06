using Zenject;
using UnityEngine;
using _Project.Scripts.Database.WeaponPrefabDatabase;

namespace _Project.Scripts.Weapon
{
    public class WeaponFactory
    {
        private readonly DiContainer _container;
        private readonly WeaponPrefabsDatabase _weaponPrefabsDatabase;
        
        public WeaponFactory(WeaponPrefabsDatabase weaponPrefabsDatabase, DiContainer container)
        {
            _container = container;
            _weaponPrefabsDatabase = weaponPrefabsDatabase;
        }

        public Weapon CreateWeapon(
            WeaponType type, 
            Vector3 position, 
            Transform parent, 
            float damageMultiplier,
            float attackSpeedMultiplier)

        {
            GameObject weaponObject = _container.InstantiatePrefab(_weaponPrefabsDatabase.Get(type), position,
                Quaternion.identity, parent);
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            weapon.SetMultipliers(damageMultiplier, attackSpeedMultiplier);

            return weapon;
        }
    }
}