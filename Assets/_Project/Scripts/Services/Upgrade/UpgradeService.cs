using System.Linq;
using _Project.Scripts.Database;
using _Project.Scripts.Configs.Upgrades;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Services.Upgrade
{
    public class UpgradeService
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradesDatabase _upgradesDatabase;

        private PlayerUpgrades Upgrades => _saveLoad.PlayerProgress.upgrades;

        public UpgradeService(ISaveLoad saveLoad, UpgradesDatabase upgradesDatabase)
        {
            _saveLoad =  saveLoad;
            _upgradesDatabase = upgradesDatabase;
        }
        
        public float GetUpgradeMultiplier(string upgradeId)
        {
            float castleHpUpgradeLevel = GetUpgradeLevel(upgradeId);
            UpgradeConfig upgradeConfig = _upgradesDatabase.GetUpgradeConfig(upgradeId);

            return 1 + upgradeConfig.statMultiplierByLevel * (castleHpUpgradeLevel - 1);
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