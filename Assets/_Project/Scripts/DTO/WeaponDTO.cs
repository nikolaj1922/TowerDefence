using System;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class WeaponDTO
    {
        public WeaponType type;
        public float damage;
        public float attackRange;
        public float attackSpeed;
        public float rotationSpeed;
    }
}