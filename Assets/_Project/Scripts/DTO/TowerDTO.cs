using System;
using _Project.Scripts.Towers;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class TowerDTO
    {
        public TowerType type;
        public WeaponType weaponType;
        public bool canBuild;
        public string iconAddress;
        public int coinPrice;
        public float occupiedRadius;
    }
}