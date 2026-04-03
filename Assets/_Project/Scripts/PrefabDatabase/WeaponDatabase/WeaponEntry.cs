using System;
using _Project.Scripts.Weapon;

namespace _Project.Scripts.PrefabDatabase.WeaponDatabase
{
    [Serializable]
    public class WeaponEntry
    {
        public WeaponType type;
        public Weapon.Weapon prefab;
    }
}