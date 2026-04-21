using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Behaviour;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace _Project.Scripts.Database.Enemies
{
    [CreateAssetMenu(menuName = "Game/Enemy Database")]
    public class EnemyPrefabsDatabase : ScriptableObject, IDatabase
    {
        [SerializeField] private EnemyEntry[] _enemyAssets;
        
        private readonly DatabaseLoader<EnemyType, Enemy> _loader =  new();
        private Dictionary<EnemyType, Enemy> Cache => _loader.Data.Cache;

        public Enemy Get(EnemyType type)
        {
            if (Cache == null)
            {
                Debug.LogError("EnemyPrefabDatabase not initialized!");
                return null;
            }
            
            if (!Cache.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"EnemyDatabase: prefab for type {type} not found!");
                return null;
            }
            
            return prefab;
        }

        public async UniTask Load()
        {
            await _loader.LoadPrefabs(
                _enemyAssets.Select(x => (x.type, x.prefab)),
                go => go.GetComponent<Enemy>()
            );
        }

        public UniTask Unload()
        {
            _loader.Unload();
            return UniTask.CompletedTask;
        }
    }
}