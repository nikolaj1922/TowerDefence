using System;
using _Project.Scripts.Weapon;

namespace _Project.Scripts.Database.WeaponDatabase
{
    [Serializable]
    public class WeaponEntry
    {
        public WeaponType type;
        public Weapon.Weapon prefab;
    }
}