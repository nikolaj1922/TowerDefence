using _Project.Scripts.PrefabDatabase.WeaponDatabase;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapons
{
    public class WeaponFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly  WeaponPrefabsDatabase _weaponPrefabsDatabase;
        
        public WeaponFactory(WeaponPrefabsDatabase weaponPrefabsDatabase, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _weaponPrefabsDatabase = weaponPrefabsDatabase;
        }
        
        public Weapon CreateWeapon(WeaponType type, Vector3 position, Transform parent)
        {
            GameObject weaponObject = 
                _instantiator.InstantiatePrefab(
                    _weaponPrefabsDatabase.Get(type),
                    position,
                    Quaternion.identity, parent
                    );
            weaponObject.transform.localScale = parent.localScale;
            Weapon weapon = weaponObject.GetComponent<Weapon>();

            return weapon;
        }
    }
}