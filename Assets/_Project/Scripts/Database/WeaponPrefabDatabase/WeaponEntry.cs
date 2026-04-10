using System;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.Database.WeaponPrefabDatabase
{
    [Serializable]
    public class WeaponEntry
    {
        public WeaponType type;
        public Weapon prefab;
    }
}