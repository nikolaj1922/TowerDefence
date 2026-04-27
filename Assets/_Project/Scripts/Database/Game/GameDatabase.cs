using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.RemoteConfigs;
using UnityEngine;

namespace _Project.Scripts.Database.Game
{
    [CreateAssetMenu(menuName = "Game/Game Database")]
    public class GameDatabase: ScriptableObject
    {
        private GameDTO _config;
        
        public GameDTO GetConfig() => _config;
        
        public void LoadConfig(IRemoteConfigService remoteConfigService)
        {
            if (!remoteConfigService.TryGetConfig<GameDTO>(GameConstants.GAME_REMOTE_CONFIG_KEY, out var config))
            {
                Debug.LogError("Failed to load game config");
                return;
            }
            
            _config = config;
        }
    }
}