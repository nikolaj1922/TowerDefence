using System.Linq;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.AssetPath;
using _Project.Scripts.Services;
using _Project.Scripts.Tower;

namespace _Project.Scripts.Repositories
{
    public class TowersRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<TowerType, TowerConfig> _towers;

        public TowersRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => _towers = _assets.LoadAll<TowerConfig>(AssetPath.Towers)
            .ToDictionary(t => t.towerType, t => t);

        public TowerConfig Get(TowerType towerType) => _towers.GetValueOrDefault(towerType);
    }
}