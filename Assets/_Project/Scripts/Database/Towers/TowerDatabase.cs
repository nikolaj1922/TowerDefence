using _Project.Scripts.Configs;
using _Project.Scripts.Towers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.Database.Towers
{
    [CreateAssetMenu(menuName = "Game/Tower Database")]
    public class TowerDatabase : ScriptableObject, IPrefabDatabase, IConfigDatabase
    {
        [SerializeField] private TowerEntry[] _towerAssets;
        
        private readonly DatabasePrefabLoader<TowerType, Tower> _prefabLoader =  new();
        private readonly DatabaseConfigLoader<TowerType, TowerConfig> _configLoader =  new();
        
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
        
        public TowerConfig GetConfig(TowerType type)
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
                _towerAssets.Select(x => (x.type, x.prefab)),
                go => go.GetComponent<Tower>()
            );
        }
        
        public async UniTask LoadConfigs()
        {
            await _configLoader.LoadAssets(
                GameConstants.TOWER_CONFIG_ASSET_LABEL,
                (x) => x.TowerType);
        }
        
        public TowerConfig[] GetBuildable() => _configLoader.Configs.Values.Where(t => t.CanBuild).ToArray();

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