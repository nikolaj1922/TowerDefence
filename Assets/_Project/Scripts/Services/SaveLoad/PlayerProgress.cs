using System;
using System.Linq;
using System.Collections.Generic;

namespace _Project.Scripts.Services.SaveLoad
{
    [Serializable]
    public class PlayerProgress
    {
        public int metaCoinsCount;
        public PlayerUpgrades upgrades;

        public BoughtUpgrade GetUpgrade(string id) =>
            upgrades.boughtUpgradeLevelsById.FirstOrDefault(x => x.id == id);

        public int GetUpgradeLevel(string id) => GetUpgrade(id)?.level ?? 0;

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
                
                upgrades.boughtUpgradeLevelsById.Add(upgrade);
            }
            else
            {
                upgrade.level = level;
            }
        }
    }

    [Serializable]
    public class PlayerUpgrades
    {
        public float castleHpMultiplier = 1f;
        public float castleAttackSpeedMultiplier = 1f;
        public float castleDamageMultiplier = 1f;
        public float towerAttackSpeedMultiplier = 1f;
        public float towerDamageMultiplier = 1f;
        
        public List<BoughtUpgrade> boughtUpgradeLevelsById = new();
    }

    [Serializable]
    public class BoughtUpgrade
    {
        public string id;
        public int level;
    }
}