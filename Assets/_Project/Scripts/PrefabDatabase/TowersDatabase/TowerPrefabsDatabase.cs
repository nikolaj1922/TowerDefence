using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Tower;

namespace _Project.Scripts.Database.TowersDatabase
{
    [CreateAssetMenu(menuName = "Game/Tower Database")]
    public class TowerPrefabsDatabase : ScriptableObject
    {
        [SerializeField] private List<TowerEntry> _towers;

        private Dictionary<TowerType, Tower.Tower> _map;

        public void Init()
        {
            _map = new Dictionary<TowerType, Tower.Tower>();

            foreach (var entry in _towers)
                _map[entry.type] = entry.prefab;
        }

        public Tower.Tower Get(TowerType type)
        {
            if (!_map.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"TowerDatabase: prefab for type {type} not found!");
                return null;
            }
            return prefab;
        }
    }
}