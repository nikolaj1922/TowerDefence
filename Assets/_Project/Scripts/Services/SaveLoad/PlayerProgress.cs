using System;
using System.Collections.Generic;

namespace _Project.Scripts.Services.SaveLoad
{
    [Serializable]
    public class PlayerProgress
    {
        public int metaCoinsCount;
        public PlayerUpgrades upgrades;
    }

    [Serializable]
    public class PlayerUpgrades
    {
        public List<BoughtUpgrade> boughtUpgradeLevelsById = new();
    }

    [Serializable]
    public class BoughtUpgrade
    {
        public string id;
        public int level;
    }
}