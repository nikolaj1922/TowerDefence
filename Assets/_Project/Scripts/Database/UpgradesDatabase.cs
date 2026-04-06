using System;
using UnityEngine;
using _Project.Scripts.Configs.Upgrades;

namespace _Project.Scripts.Database
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/All Upgrades Config")]
    public class UpgradesDatabase : ScriptableObject
    {
        public UpgradeBaseConfig[] upgrades; 
    }
}