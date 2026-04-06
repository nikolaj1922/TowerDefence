using System;

namespace _Project.Scripts.Configs.Upgrades
{
    [Serializable]
    public class UpgradeLevel
    {
        public int level;
        public int price;
        public float multiplier;
        public string description;
    }
}