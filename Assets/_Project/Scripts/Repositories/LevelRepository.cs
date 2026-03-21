using _Project.Scripts.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.Infrastructure.AssetPath;

namespace _Project.Scripts.Repositories
{
    public class LevelRepository
    {
        public LevelConfig LevelConfig { get; private set; }
        private readonly IAssetProvider _assets;
        
        public LevelRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => LevelConfig = _assets.Load<LevelConfig>(AssetPath.Level);
    }
}