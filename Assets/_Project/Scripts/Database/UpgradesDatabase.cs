using System.Linq;
using UnityEngine;
using _Project.Scripts.Configs.Upgrades;

namespace _Project.Scripts.Database
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/All Upgrades Config")]
    public class UpgradesDatabase : ScriptableObject
    {
        public UpgradeConfig[] upgrades;
        
        public UpgradeConfig GetUpgradeConfig(string id) => upgrades.FirstOrDefault(x => x.id == id);
    }
}