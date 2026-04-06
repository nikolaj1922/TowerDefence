using System;
using _Project.Scripts.Weapon;

namespace _Project.Scripts.Database.WeaponPrefabDatabase
{
    [Serializable]
    public class WeaponEntry
    {
        public WeaponType type;
        public Weapon.Weapon prefab;
    }
}