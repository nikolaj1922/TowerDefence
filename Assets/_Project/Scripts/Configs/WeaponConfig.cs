using _Project.Scripts.Weapon;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Weapon Config", fileName = "Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        public WeaponType weaponType;
        
        public float damage;
        public float attackRange;
        public float attackSpeed;
        public float rotationSpeed;
    }
}