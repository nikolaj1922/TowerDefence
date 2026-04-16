using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using _Project.Scripts.Towers;
using _Project.Scripts.Configs;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.ConfigRepositories
{
    public class TowerConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<TowerType, TowerConfig> _towers;

        public TowerConfigsRepository(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            try
            {
                TowerConfig[] configs = 
                    await _assets.LoadByLabel<TowerConfig>(GameConstants.TOWER_CONFIG_ASSET_LABEL);

                if (configs == null || configs.Length == 0)
                {
                    Debug.LogWarning($"Tower configs could not be found.");
                    _towers = new Dictionary<TowerType, TowerConfig>();
                    return;
                }

                _towers = configs.ToDictionary(t => t.TowerType, t => t);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Tower configs loading failed: {ex.Message}.");
                _towers = new Dictionary<TowerType, TowerConfig>();
            }
        }

        public TowerConfig Get(TowerType towerType)
        {
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