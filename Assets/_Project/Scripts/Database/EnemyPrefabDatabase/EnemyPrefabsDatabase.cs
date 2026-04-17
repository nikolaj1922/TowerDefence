using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Behaviour;

namespace _Project.Scripts.Database.EnemyPrefabDatabase
{
    [CreateAssetMenu(menuName = "Game/Enemy Database")]
    public class EnemyPrefabsDatabase : ScriptableObject
    {
        [SerializeField] private List<EnemyEntry> _enemies;

        private Dictionary<EnemyType, Enemy> _map;

        public void Init()
        {
            _map = new Dictionary<EnemyType, Enemy>();

            foreach (var entry in _enemies)
                _map[entry.type] = entry.prefab;
        }

        public Enemy Get(EnemyType type)
        {
            if (!_map.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"EnemyDatabase: prefab for type {type} not found!");
                return null;
            }
            return prefab;
        }
    }
}