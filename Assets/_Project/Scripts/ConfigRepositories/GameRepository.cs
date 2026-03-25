using _Project.Scripts.Configs;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.AssetPath;

namespace _Project.Scripts.ConfigRepositories
{
    public class GameRepository
    {
        public GameConfig GameConfig { get; private set; }
        private readonly IAssetProvider _assets;
        
        public GameRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => GameConfig = _assets.Load<GameConfig>(AssetPath.Game);
    }
}