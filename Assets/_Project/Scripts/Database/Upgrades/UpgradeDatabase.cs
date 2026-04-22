using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Upgrades
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/All Upgrades Config")]
    public class UpgradeDatabase : ScriptableObject, IConfigDatabase
    {
        [SerializeField] private AssetReference[] _upgradeAssets;
        
        private readonly DatabaseConfigLoader<string, UpgradeConfig> _configLoader = new();

        public UpgradeConfig GetConfig(string id)
        {
            if (_configLoader.Configs == null)
            {
                Debug.LogError("WeaponDatabase not initialized!");
                return null;
            }
            
            if (_configLoader.Configs.TryGetValue(id, out UpgradeConfig config))
                return config;
            
            Debug.LogError($"Can't find upgrade config {id}");
            return null;
        }

        public UpgradeConfig[] GetUpgrades() => _configLoader.Configs.Values.ToArray();

        public async UniTask LoadConfigs()
        {
            await _configLoader.LoadAssets(
                GameConstants.UPGRADE_CONFIG_ASSET_LABEL,
                (x) => x.id);
        }

        public UniTask UnloadConfigs()
        {
            _configLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}