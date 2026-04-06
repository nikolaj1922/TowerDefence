using UnityEngine;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Configs.Upgrades.Castle
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/Castle Damage")]
    public class CastleDamageUpgradeConfig : UpgradeBaseConfig
    {
        public override void OnBuy(PlayerProgress progress, float multiplier)
        {
            progress.upgrades.castleDamageMultiplier = multiplier;
        }
    }
}