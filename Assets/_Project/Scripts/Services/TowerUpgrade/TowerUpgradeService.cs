using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Database;
using _Project.Scripts.Database.Upgrades;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Services.TowerUpgrade
{
    public class TowerUpgradeService: ITowerUpgradeService
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradeDatabase _upgradeDatabase;

        private PlayerUpgrades Upgrades => _saveLoad.PlayerProgress.upgrades;

        public TowerUpgradeService(ISaveLoad saveLoad, UpgradeDatabase upgradeDatabase)
        {
            _saveLoad = saveLoad;
            _upgradeDatabase = upgradeDatabase;
        }
        
        public float GetUpgradeMultiplier(string upgradeId)
        {
            float upgradeLevel = GetUpgradeLevel(upgradeId);
            UpgradeConfig upgradeConfig = _upgradeDatabase.GetConfig(upgradeId);

            return 1 + upgradeConfig.statMultiplierByLevel * (upgradeLevel - 1);
        }
        
        public int GetUpgradeLevel(string id) => GetUpgrade(id)?.level ?? 1;

        public void SetUpgradeLevel(string id, int level)
        {
            BoughtUpgrade upgrade = GetUpgrade(id);

            if (upgrade == null)
            {
                upgrade = new BoughtUpgrade()
                {
                    id = id,
                    level = level
                };
                
                Upgrades.boughtUpgradeLevelsById.Add(upgrade);
            }
            else
            {
                upgrade.level = level;
            }
        }
        
        private BoughtUpgrade GetUpgrade(string id) =>
            Upgrades.boughtUpgradeLevelsById.FirstOrDefault(x => x.id == id);
    }
}