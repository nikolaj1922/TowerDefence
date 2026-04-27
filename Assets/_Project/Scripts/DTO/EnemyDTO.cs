using System;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class EnemyDTO
    {
        public EnemyType type;
        public float speed;
        public float health;
        public float damage;
        public float attackRange;
        public float attackCooldown;
        public int coinsReward;
    }
}