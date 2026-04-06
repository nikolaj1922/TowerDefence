using _Project.Scripts.Database;
using _Project.Scripts.Configs.Upgrades;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Services.Upgrade
{
    public class UpgradeService
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradesDatabase _upgradesDatabase;

        private PlayerProgress Progress => _saveLoad.PlayerProgress;

        public UpgradeService(ISaveLoad saveLoad, UpgradesDatabase upgradesDatabase)
        {
            _saveLoad =  saveLoad;
            _upgradesDatabase = upgradesDatabase;
        }
        
        public float GetUpgradeMultiplier(string upgradeId)
        {
            float castleHpUpgradeLevel = Progress.GetUpgradeLevel(upgradeId);
            UpgradeConfig upgradeConfig = _upgradesDatabase.GetUpgradeConfig(upgradeId);

            return 1 + upgradeConfig.statMultiplierByLevel * (castleHpUpgradeLevel - 1);
        }
    }
}