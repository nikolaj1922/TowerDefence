using _Project.Scripts.Services.SaveLoad;
using UnityEngine;

namespace _Project.Scripts.Configs.Upgrades.Castle
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/Castle Hp")]
    public class CastleHpUpgradeConfig : UpgradeBaseConfig
    {
        public override void OnBuy(PlayerProgress progress, float multiplier)
        {
            progress.upgrades.castleHpMultiplier = multiplier;
        }
    }
}