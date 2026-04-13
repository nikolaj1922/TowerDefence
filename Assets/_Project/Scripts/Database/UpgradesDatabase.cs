using System.Linq;
using _Project.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.Database
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/All Upgrades Config")]
    public class UpgradesDatabase : ScriptableObject
    {
        public UpgradeConfig[] upgrades;
        
        public UpgradeConfig GetUpgradeConfig(string id) => upgrades.FirstOrDefault(x => x.id == id);
    }
}