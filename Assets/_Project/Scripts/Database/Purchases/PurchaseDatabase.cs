using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.DTO;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.RemoteConfigs;
using UnityEngine;

namespace _Project.Scripts.Database.Purchases
{
    [CreateAssetMenu(menuName = "Configs/Purchases")]
    public class PurchaseDatabase : ScriptableObject
    {
        private readonly Dictionary<string, PurchaseDTO> _configs = new();
        
        public PurchaseDTO GetPurchaseConfig(string id)
        {
            if (!_configs.TryGetValue(id, out PurchaseDTO config))
            {
                Debug.LogError($"Can't find purchase config {id}");
                return null;
            }
               
            return config;
        }
        
        public PurchaseDTO[] GetPurchases() => _configs.Values.ToArray();

        public void LoadConfigs(IRemoteConfigService remoteConfigService)
        {
            if(!remoteConfigService.TryGetConfig<RemoteConfig<PurchaseDTO>>(GameConstants.PURCHASES_REMOTE_CONFIG_KEY,
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