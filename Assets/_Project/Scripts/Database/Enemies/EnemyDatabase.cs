using System.Collections.Generic;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Behaviour;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.RemoteConfigs;

namespace _Project.Scripts.Database.Enemies
{
    [CreateAssetMenu(menuName = "Game/Enemy Database")]
    public class EnemyDatabase : ScriptableObject, IPrefabDatabase
    {
        [SerializeField] private EnemyEntry[] _enemyAssets;
        
        private readonly DatabasePrefabLoader<EnemyType, Enemy> _prefabLoader = new();
        private readonly Dictionary<EnemyType, EnemyDTO> _configs = new();

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
        
        public EnemyDTO GetConfig(EnemyType type)
        {
            if (!_configs.TryGetValue(type, out var config))
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

        public void LoadConfig(IRemoteConfigService remoteConfigService)
        {
            if (!remoteConfigService.TryGetConfig<RemoteConfig<EnemyDTO>>(GameConstants.ENEMY_REMOTE_CONFIG_KEY, out var config))
            {
                Debug.LogError("Loading enemy configs failed");
                return;
            }

            foreach (var dto in config.items)
                _configs[dto.type] = dto;
        }

        public UniTask UnloadPrefabs()
        {
            _prefabLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}