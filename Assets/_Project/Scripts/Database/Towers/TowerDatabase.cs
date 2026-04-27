using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Towers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.RemoteConfigs;

namespace _Project.Scripts.Database.Towers
{
    [CreateAssetMenu(menuName = "Game/Tower Database")]
    public class TowerDatabase : ScriptableObject, IPrefabDatabase
    {
        [SerializeField] private TowerEntry[] _towerAssets;
        
        private readonly DatabasePrefabLoader<TowerType, Tower> _prefabLoader =  new();
        private readonly Dictionary<TowerType, TowerDTO> _configs =  new();
        
        public Tower GetPrefab(TowerType type)
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
        
        public TowerDTO GetConfig(TowerType type)
        {
            if (_configs == null)
            {
                Debug.LogError("Configs not initialized!");
                return null;
            }
            
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
                _towerAssets.Select(x => (x.type, x.prefab)),
                go => go.GetComponent<Tower>()
            );
        }
        
        public void LoadConfig(IRemoteConfigService remoteConfigService)
        {
            if (!remoteConfigService.TryGetConfig<RemoteConfig<TowerDTO>>(GameConstants.TOWER_REMOTE_CONFIG_KEY, out var config))
            {
                Debug.LogError("Loading tower configs failed");
                return;
            }

            foreach (var dto in config.items)
                _configs[dto.type] = dto;
        }
        
        public TowerDTO[] GetBuildable() => _configs.Values.Where(t => t.canBuild).ToArray();

        public UniTask UnloadPrefabs()
        {
            _prefabLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}