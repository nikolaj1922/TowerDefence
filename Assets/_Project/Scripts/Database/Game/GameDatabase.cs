using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Database.Game
{
    [CreateAssetMenu(menuName = "Game/Game Database")]
    public class GameDatabase: ScriptableObject, IConfigDatabase
    {
        private const string GAME_CONFIG_KEY = "Game";
        
        private readonly DatabaseConfigLoader<string, GameConfig> _configLoader = new();
        
        public GameConfig GetConfig() => _configLoader.Configs[GAME_CONFIG_KEY];
        
        public async UniTask LoadConfigs()
        {
            await _configLoader.LoadAssets(
                GameConstants.GAME_CONFIG_ASSET_LABEL,
                (x) => GAME_CONFIG_KEY);
        }

        public UniTask UnloadConfigs()
        {
            _configLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}