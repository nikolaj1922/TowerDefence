using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Configs/Enemy")]
    public class EnemyConfig : ScriptableObject
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