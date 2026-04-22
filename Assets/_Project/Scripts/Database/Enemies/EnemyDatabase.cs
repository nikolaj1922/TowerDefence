using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Behaviour;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.Database.Enemies
{
    [CreateAssetMenu(menuName = "Game/Enemy Database")]
    public class EnemyDatabase : ScriptableObject, IPrefabDatabase, IConfigDatabase
    {
        [SerializeField] private EnemyEntry[] _enemyAssets;
        
        private readonly DatabasePrefabLoader<EnemyType, Enemy> _prefabLoader = new();
        private readonly DatabaseConfigLoader<EnemyType, EnemyConfig> _configLoader = new();

        public Enemy GetPrefab(EnemyType type)
        {
            if (_prefabLoader.Prefabs == null)
            {
                Debug.LogError("Prefabs not initialized!");
                return null;
            }
            
            if (!_prefabLoader.Prefabs.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"Prefab not found for type: {type}");
                return null;
            }
            
            return prefab;
        }
        
        public EnemyConfig GetConfig(EnemyType type)
        {
            if (_configLoader.Configs == null)
            {
                Debug.LogError("Configs not initialized!");
                return null;
            }
            
            if (!_configLoader.Configs.TryGetValue(type, out var config))
            {
                Debug.LogError($"Config not found for type: {type}");
                return null;
            }

            return config;
        }

        public async UniTask LoadPrefabs()
        {
            await _prefabLoader.LoadAssets(
                _enemyAssets.Select(x => (x.type, x.prefab)),
                go => go.GetComponent<Enemy>()
            );
        }

        public async UniTask LoadConfigs()
        {
            await _configLoader.LoadAssets(
                GameConstants.ENEMY_CONFIG_ASSET_LABEL,
                (x) => x.Type);
        }

        public UniTask UnloadPrefabs()
        {
            _prefabLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }

        public UniTask UnloadConfigs()
        {
            _configLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}