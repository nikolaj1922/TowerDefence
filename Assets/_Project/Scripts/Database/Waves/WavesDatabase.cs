using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.RemoteConfigs;
using UnityEngine;

namespace _Project.Scripts.Database.Waves
{
    [CreateAssetMenu(menuName = "Game/Waves Database")]
    public class WavesDatabase: ScriptableObject
    {
        private WaveDTO[] _config;
        
        public WaveDTO[] GetConfig() => _config;
        
        public void LoadConfig(IRemoteConfigService remoteConfigService)
        {
            if (!remoteConfigService.TryGetConfig<RemoteConfig<WaveDTO>>(GameConstants.WAVES_REMOTE_CONFIG_KEY, out var config))
            {
                Debug.LogError("Failed to load waves config");
                return;
            }
            
            _config = config.items;
        }
    }
}