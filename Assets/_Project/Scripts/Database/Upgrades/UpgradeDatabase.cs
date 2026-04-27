using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.RemoteConfigs;
using UnityEngine;

namespace _Project.Scripts.Database.Upgrades
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/All Upgrades Config")]
    public class UpgradeDatabase : ScriptableObject
    {
        private readonly Dictionary<string, UpgradeDTO> _configs = new();

        public UpgradeDTO GetConfig(string id)
        {
            if (!_configs.TryGetValue(id, out UpgradeDTO config))
            {
                Debug.LogError($"Can't find upgrade config {id}");
                return null;
            }
               
            return config;
        }

        public UpgradeDTO[] GetUpgrades() => _configs.Values.ToArray();

        public void LoadConfigs(IRemoteConfigService remoteConfigService)
        {
            if (!remoteConfigService.TryGetConfig<RemoteConfig<UpgradeDTO>>(GameConstants.UPGRADES_REMOTE_CONFIG_KEY,
                    out var config))
            {
                Debug.LogError("Failed to load upgrade configs");
                return;
            }

            foreach (var dto in config.items)
                _configs[dto.id] = dto;
        }
    }
}