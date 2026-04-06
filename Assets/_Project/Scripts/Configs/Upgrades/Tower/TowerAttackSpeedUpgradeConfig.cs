using UnityEngine;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Configs.Upgrades.Tower
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/Tower Attack Speed")]
    public class TowerAttackSpeedUpgradeConfig : UpgradeBaseConfig
    {
        public override void OnBuy(PlayerProgress progress, float multiplier)
        {
            progress.upgrades.towerAttackSpeedMultiplier = multiplier;
        }
    }
}