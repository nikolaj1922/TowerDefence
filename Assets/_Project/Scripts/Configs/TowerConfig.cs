using UnityEngine;
using _Project.Scripts.Tower;
using _Project.Scripts.Weapon;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Tower Config", fileName = "Tower")]
    public class TowerConfig : ScriptableObject
    {
        public TowerType towerType;
        public WeaponType weaponType;

        public bool canBuild;
        public Sprite icon;
        public int coinPrice;
        public float occupiedRadius;
    }
}