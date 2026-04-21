using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Database.Enemies
{
    public class EnemyConfigsDatabase : IDatabase
    {
        private readonly IAssetProvider _assets;
        private Dictionary<EnemyType, EnemyConfig> _enemies;

        public EnemyConfigsDatabase(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            EnemyConfig[] configs =
                await _assets.LoadByLabel<EnemyConfig>(GameConstants.ENEMY_CONFIG_ASSET_LABEL);

            if (configs == null || configs.Length == 0)
            {
                Debug.LogWarning($"Enemy configs could not be found.");
                return;
            }

            _enemies = configs.ToDictionary(c => c.Type, c => c);
        }
      

        public EnemyConfig Get(EnemyType enemyType)
        {
            if (_enemies == null)
            {
                Debug.LogError("Enemies config repository not initialized.");
                return null;
            }
            
            if (!_enemies.TryGetValue(enemyType, out var config))
            {
                Debug.LogError($"Enemy config not found for type: {enemyType}");
                return null;
            }

            return config;
        }
    }
}