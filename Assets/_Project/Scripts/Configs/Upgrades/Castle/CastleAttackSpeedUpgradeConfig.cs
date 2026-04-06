using UnityEngine;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Configs.Upgrades.Castle
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/Castle Attack Speed")]
    public class CastleAttackSpeedUpgradeConfig : UpgradeBaseConfig
    {
        public override void OnBuy(PlayerProgress progress, float multiplier)
        {
            progress.upgrades.castleAttackSpeedMultiplier = multiplier;
        }
    }
}