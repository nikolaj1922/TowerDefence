using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Weapons;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.ConfigRepositories
{
    public class WeaponConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<WeaponType, WeaponConfig> _weapons;

        public WeaponConfigsRepository(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            try
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
            catch(Exception ex)
            {
                Debug.LogError($"Weapon configs loading failed: {ex.Message}");
                _weapons = new Dictionary<WeaponType, WeaponConfig>();
            }
        }
        
        public WeaponConfig Get(WeaponType towerType)
        {
            if (!_weapons.TryGetValue(towerType, out var config))
            {
                Debug.LogError($"Weapon config not found for type: {towerType}");
                return null;
            }

            return config;
        }
    }
}