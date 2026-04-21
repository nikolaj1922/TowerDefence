using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Towers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Database.Towers
{
    public class TowerConfigsDatabase : IDatabase
    {
        private readonly IAssetProvider _assets;
        private Dictionary<TowerType, TowerConfig> _towers;

        public TowerConfigsDatabase(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            TowerConfig[] configs = 
                await _assets.LoadByLabel<TowerConfig>(GameConstants.TOWER_CONFIG_ASSET_LABEL);

            if (configs == null || configs.Length == 0)
            {
                Debug.LogWarning($"Tower configs could not be found.");
                return;
            }

            _towers = configs.ToDictionary(t => t.TowerType, t => t);
        }

        public TowerConfig Get(TowerType towerType)
        {
            if (_towers == null)
            {
                Debug.LogError("Towers config repository not initialized.");
                return null;
            }
            
            if (!_towers.TryGetValue(towerType, out var config))
            {
                Debug.LogError($"Tower config not found for type: {towerType}");
                return null;
            }

            return config;
        }

        public TowerConfig[] GetBuildable() => _towers.Values.Where(t => t.CanBuild).ToArray();
    }
}