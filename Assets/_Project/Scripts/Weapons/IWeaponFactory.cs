using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public interface IWeaponFactory
    {
        Weapon CreateWeapon(
            WeaponType type, 
            Vector3 position, 
            Transform parent, 
            float damageMultiplier,
            float attackSpeedMultiplier);
    }
}