using System;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class UpgradeDTO
    {
        public string id;
        public string title;
        public string description;
        public string iconAddress;
        public int basePrice;
        public int priceMultiplierByLevel;
        public float statMultiplierByLevel;
    }
}