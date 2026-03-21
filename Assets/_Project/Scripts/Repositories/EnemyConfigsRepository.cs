using System.Linq;
using System.Collections.Generic;
using _Project.Scripts.Enemy;
using _Project.Scripts.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.Infrastructure.AssetPath;

namespace _Project.Scripts.Repositories
{
    public class EnemyConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<EnemyType, EnemyConfig> _enemies;

        public EnemyConfigsRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => _enemies = _assets.LoadAll<EnemyConfig>(AssetPath.Enemies).ToDictionary(l => l.type, l => l);

        public EnemyConfig ForEnemy(EnemyType enemyType) => _enemies.GetValueOrDefault(enemyType);
    }
}