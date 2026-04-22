using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Constants;
using UnityEngine;

namespace _Project.Scripts.Services.SaveLoad
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerUpgrades upgrades;
        [SerializeField] private int _metaCoinsCount;

        public int MetaCoinsCount => _metaCoinsCount;

        public void AddMetaCoins(int amount)
        {
            if (amount <= 0)
                return;
            
            _metaCoinsCount = Mathf.Min(_metaCoinsCount + amount, GameConstants.MAX_META_COIN_COUNT);
        }

        public void SpendMetaCoins(int amount)
        {
            if (amount <= 0 || _metaCoinsCount < amount)
                return;
            
            _metaCoinsCount = Mathf.Max(0,  _metaCoinsCount - amount);
        }
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