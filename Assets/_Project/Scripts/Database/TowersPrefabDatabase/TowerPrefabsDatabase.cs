using System.Collections.Generic;
using _Project.Scripts.Towers;
using UnityEngine;

namespace _Project.Scripts.Database.TowersPrefabDatabase
{
    [CreateAssetMenu(menuName = "Game/Tower Database")]
    public class TowerPrefabsDatabase : ScriptableObject
    {
        [SerializeField] private List<TowerEntry> _towers;

        private Dictionary<TowerType, Tower> _map;

        public void Init()
        {
            _map = new Dictionary<TowerType, Tower>();

            foreach (var entry in _towers)
                _map[entry.type] = entry.prefab;
        }

        public Tower Get(TowerType type)
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