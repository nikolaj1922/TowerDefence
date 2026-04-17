using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.GameConstants;
using UnityEngine;

namespace _Project.Scripts.Services.SaveLoad
{
    [Serializable]
    public class PlayerProgress
    {
        public int MetaCoinsCount
        {
            get => _metaCoinsCount;
            set
            {
                switch (value)
                {
                    case < 0:
                        _metaCoinsCount = 0;
                        return;
                    case > GameConstants.MAX_META_COIN_COUNT:
                        _metaCoinsCount = GameConstants.MAX_META_COIN_COUNT;
                        return;
                    default:
                        _metaCoinsCount = value;
                        break;
                }
            }
        }

        [SerializeField] private int _metaCoinsCount;
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