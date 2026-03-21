using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.AssetPath;

namespace _Project.Scripts.Services.LevelConfigRepository
{
    public class LevelConfigRepository
    {
        public LevelConfig LevelConfig { get; private set; }
        private readonly IAssetProvider _assets;
        
        public LevelConfigRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => LevelConfig = _assets.Load<LevelConfig>(AssetPath.Level);
    }
}