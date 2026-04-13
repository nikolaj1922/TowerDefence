using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Database;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Services.TowerUpgrade
{
    public class TowerUpgradeService
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradesDatabase _upgradesDatabase;

        private PlayerUpgrades Upgrades => _saveLoad.PlayerProgress.upgrades;

        public TowerUpgradeService(ISaveLoad saveLoad, UpgradesDatabase upgradesDatabase)
        {
            _saveLoad = saveLoad;
            _upgradesDatabase = upgradesDatabase;
        }
        
        public float GetUpgradeMultiplier(string upgradeId)
        {
            float upgradeLevel = GetUpgradeLevel(upgradeId);
            UpgradeConfig upgradeConfig = _upgradesDatabase.GetUpgradeConfig(upgradeId);

            return 1 + upgradeConfig.statMultiplierByLevel * (upgradeLevel - 1);
        }
        
        private BoughtUpgrade GetUpgrade(string id) =>
            Upgrades.boughtUpgradeLevelsById.FirstOrDefault(x => x.id == id);

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
    }
}