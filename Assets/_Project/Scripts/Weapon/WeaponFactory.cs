using Zenject;
using UnityEngine;
using _Project.Scripts.Database.WeaponDatabase;

namespace _Project.Scripts.Weapon
{
    public class WeaponFactory
    {
        private readonly DiContainer _container;
        private readonly  WeaponPrefabsDatabase _weaponPrefabsDatabase;
        
        public WeaponFactory(WeaponPrefabsDatabase weaponPrefabsDatabase, DiContainer container)
        {
            _container = container;
            _weaponPrefabsDatabase = weaponPrefabsDatabase;
        }
        
        public Weapon CreateWeapon(WeaponType type, Vector3 position, Transform parent)
        {
            GameObject weaponObject = _container.InstantiatePrefab(_weaponPrefabsDatabase.Get(type), position,
                Quaternion.identity, parent);
            Weapon weapon = weaponObject.GetComponent<Weapon>();

            return weapon;
        }
    }
}