using System;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.ConfigRepositories
{
    public class EnemyConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<EnemyType, EnemyConfig> _enemies;

        public EnemyConfigsRepository(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            try
            {
                EnemyConfig[] configs =
                    await _assets.LoadByLabel<EnemyConfig>(GameConstants.ENEMY_CONFIG_ASSET_LABEL);

                if (configs == null || configs.Length == 0)
                {
                    Debug.LogWarning($"Enemy configs could not be found.");
                    _enemies = new Dictionary<EnemyType, EnemyConfig>();
                    return;
                }

                _enemies = configs.ToDictionary(c => c.Type, c => c);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Enemy configs loading failed: {ex.Message}");
                _enemies = new Dictionary<EnemyType, EnemyConfig>();
            }
        }
      

        public EnemyConfig Get(EnemyType enemyType)
        {
            if (!_enemies.TryGetValue(enemyType, out var config))
            {
                Debug.LogError($"Enemy config not found for type: {enemyType}");
                return null;
            }

            return config;
        }
    }
}