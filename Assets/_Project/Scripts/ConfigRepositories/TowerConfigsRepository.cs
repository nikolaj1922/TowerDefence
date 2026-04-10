using System.Linq;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.AssetPath;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Towers;

namespace _Project.Scripts.ConfigRepositories
{
    public class TowerConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<TowerType, TowerConfig> _towers;

        public TowerConfigsRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => _towers = _assets.LoadAll<TowerConfig>(AssetPath.TOWERS)
            .ToDictionary(t => t.TowerType, t => t);

        public TowerConfig Get(TowerType towerType) => _towers.GetValueOrDefault(towerType);

        public TowerConfig[] GetBuildable() => _towers.Values.Where(t => t.CanBuild).ToArray();
    }
}