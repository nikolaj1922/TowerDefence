using System.Linq;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.AssetPath;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.ConfigRepositories
{
    public class WeaponConfigsRepository
    {
        private readonly IAssetProvider _assets;
        private Dictionary<WeaponType, WeaponConfig> _weapons;

        public WeaponConfigsRepository(IAssetProvider assets) => _assets = assets;

        public void Load() => _weapons = _assets.LoadAll<WeaponConfig>(AssetPath.WEAPONS)
            .ToDictionary(t => t.weaponType, t => t);

        public WeaponConfig Get(WeaponType towerType) => _weapons.GetValueOrDefault(towerType);
    }
}