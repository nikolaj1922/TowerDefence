using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Configs;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.AssetPath;

namespace _Project.Scripts.ConfigRepositories
{
    public class GameRepository
    {
        private readonly IAssetProvider _assets;
        
        public GameConfig GameConfig { get; private set; }

        public GameRepository(IAssetProvider assets) => _assets = assets;

        public async UniTask Load()
        {
            try
            {
                GameConfig config = await _assets.Load<GameConfig>(AssetPath.GAME_CONFIG);

                if (config == null)
                {
                    Debug.LogWarning($"Game config could not be found.");
                    return;
                }
            
                GameConfig = config;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Game config loading failed: {ex.Message}");
            }
        }
    }
}