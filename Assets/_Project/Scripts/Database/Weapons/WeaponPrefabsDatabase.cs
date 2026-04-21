using System.Collections.Generic;
using _Project.Scripts.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Weapons
{
    [CreateAssetMenu(menuName = "Game/Weapon Database")]
    public class WeaponPrefabsDatabase : ScriptableObject, IDatabase
    {
        [SerializeField] private WeaponEntry[] _weaponAssets;

        private Dictionary<WeaponType, Weapon> _cache;

        public Weapon Get(WeaponType type)
        {
            if (_cache == null)
            {
                Debug.LogError("WeaponPrefabsDatabase not initialized!");
                return null;
            }
            
            if (!_cache.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"WeaponPrefabsDatabase: prefab for type {type} not found!");
                return null;
            }
            
            return prefab;
        }
        
        public async UniTask Load()
        {
            List<UniTask<GameObject>> tasks = new();
            _cache = new Dictionary<WeaponType, Weapon>();

            foreach (WeaponEntry entry in _weaponAssets)
                tasks.Add(Addressables.LoadAssetAsync<GameObject>(entry.prefab).ToUniTask());
            
            GameObject[] weapons = await UniTask.WhenAll(tasks);

            for (int i = 0; i < _weaponAssets.Length; i++)
            {
                WeaponEntry entry = _weaponAssets[i];
                Weapon weapon = weapons[i].GetComponent<Weapon>();
                
                if (weapon == null)
                {
                    Debug.LogError($"Failed to load weapon {entry.type}");
                    continue;
                }
                
                _cache[entry.type] = weapon;
            }
        }
    }
}