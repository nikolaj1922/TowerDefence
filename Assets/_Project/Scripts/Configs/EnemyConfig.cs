using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Configs/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public int CoinsReward { get; private set; }
    }
}