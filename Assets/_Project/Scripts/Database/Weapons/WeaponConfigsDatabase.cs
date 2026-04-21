using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Database.Weapons
{
    public class WeaponConfigsDatabase : IDatabase
    {
        private readonly IAssetProvider _assets;
        private Dictionary<WeaponType, WeaponConfig> _weapons;

        public WeaponConfigsDatabase(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            WeaponConfig[] configs = 
                await _assets.LoadByLabel<WeaponConfig>(GameConstants.WEAPON_CONFIG_ASSET_LABEL);

            if (configs == null || configs.Length == 0)
            {
                Debug.LogWarning($"Weapon configs could not be found.");
                _weapons = new Dictionary<WeaponType, WeaponConfig>();
                return;
            }

            _weapons = configs.ToDictionary(c => c.WeaponType, c => c);
        }
        
        public WeaponConfig Get(WeaponType towerType)
        {
            if (_weapons == null)
            {
                Debug.LogError("Weapons config repository not initialized.");
                return null;
            }
            
            if (!_weapons.TryGetValue(towerType, out var config))
            {
                Debug.LogError($"Weapon config not found for type: {towerType}");
                return null;
            }

            return config;
        }
    }
}