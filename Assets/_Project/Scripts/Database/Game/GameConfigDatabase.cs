using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Database.Game
{
    public class GameConfigDatabase : IDatabase
    {
        private readonly IAssetProvider _assets;
        
        public GameConfig GameConfig { get; private set; }

        public GameConfigDatabase(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            GameConfig[] config = 
                await _assets.LoadByLabel<GameConfig>(GameConstants.GAME_CONFIG_ASSET_LABEL);

            if (config == null)
            {
                Debug.LogWarning($"Game config could not be found.");
                return;
            }
            
            GameConfig = config[0];
        }
    }
}