using _Project.Scripts.Towers;
using UnityEngine;
using _Project.Scripts.Weapons;

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