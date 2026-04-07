using _Project.Scripts.PrefabDatabase.WeaponDatabase;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapons
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
            weaponObject.transform.localScale = parent.localScale;
            Weapon weapon = weaponObject.GetComponent<Weapon>();

            return weapon;
        }
    }
}