using System;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.PrefabDatabase.WeaponDatabase
{
    [Serializable]
    public class WeaponEntry
    {
        public WeaponType type;
        public Weapon prefab;
    }
}