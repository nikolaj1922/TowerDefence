using System.Collections.Generic;
using _Project.Scripts.Towers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Towers
{
    [CreateAssetMenu(menuName = "Game/Tower Database")]
    public class TowerPrefabsDatabase : ScriptableObject, IDatabase
    {
        [SerializeField] private List<TowerEntry> _towerAssets;

        private Dictionary<TowerType, Tower> _cache;
        
        public Tower Get(TowerType type)
        {
            if (_cache == null)
            {
                Debug.LogError($"TowerPrefabsDatabase not initialized!");
                return null;
            }
            
            if (!_cache.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"TowerPrefabsDatabase: prefab for type {type} not found!");
                return null;
            }
            return prefab;
        }

        public async UniTask Load()
        {
            List<UniTask<GameObject>> tasks = new();
            _cache = new Dictionary<TowerType, Tower>();

            foreach (TowerEntry entry in _towerAssets)
                tasks.Add(Addressables.LoadAssetAsync<GameObject>(entry.prefab).ToUniTask());

            GameObject[] towers = await UniTask.WhenAll(tasks);

            for (int i = 0; i < _towerAssets.Count; i++)
            {
                Tower tower = towers[i].GetComponent<Tower>();
                TowerEntry entry = _towerAssets[i];

                if (tower == null)
                {
                    Debug.LogError("Failed to load tower: " + entry.type);
                    continue;
                }
                    
                _cache[entry.type] = tower;
            }
        }
    }
}