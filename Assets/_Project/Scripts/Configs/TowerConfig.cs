using UnityEngine;
using _Project.Scripts.Tower;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Tower Config", fileName = "Tower")]
    public class TowerConfig : ScriptableObject
    {
        public TowerType towerType;

        public float damage;
        public float attackRange;
        public float attackSpeed;
        public float rotationSpeed;

        public float price;
        public float holdDistance;
    }
}