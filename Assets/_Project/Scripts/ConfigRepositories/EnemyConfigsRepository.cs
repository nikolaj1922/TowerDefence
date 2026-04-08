using System.Linq;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.AssetPath;

namespace _Project.Scripts.ConfigRepositories
{
    public class EnemyConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<EnemyType, EnemyConfig> _enemies;

        public EnemyConfigsRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => _enemies = _assets.LoadAll<EnemyConfig>(AssetPath.ENEMIES).ToDictionary(l => l.Type, l => l);

        public EnemyConfig Get(EnemyType enemyType) => _enemies.GetValueOrDefault(enemyType);
    }
}