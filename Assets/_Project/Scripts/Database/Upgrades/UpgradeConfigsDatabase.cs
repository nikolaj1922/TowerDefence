using System.Collections.Generic;
using _Project.Scripts.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Upgrades
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/All Upgrades Config")]
    public class UpgradeConfigsDatabase : ScriptableObject, IDatabase
    {
        [SerializeField] private AssetReference[] _upgradeAssets;
        
        public Dictionary<string, UpgradeConfig> Upgrades { get; private set; }

        public UpgradeConfig GetUpgradeConfig(string id)
        {
            if (Upgrades == null)
            {
                Debug.LogError("WeaponDatabase not initialized!");
                return null;
            }
            
            if (Upgrades.TryGetValue(id, out UpgradeConfig config))
                return config;
            
            Debug.LogError($"Can't find upgrade config {id}");
            return null;
        }
        
        public async UniTask Load()
        {
            Upgrades = new Dictionary<string, UpgradeConfig>();

            List<UniTask<UpgradeConfig>> handles = new();

            foreach (var reference in _upgradeAssets)
                handles.Add(Addressables.LoadAssetAsync<UpgradeConfig>(reference).ToUniTask());

            UpgradeConfig[] result = await UniTask.WhenAll(handles);

            foreach (var config in result)
                Upgrades[config.id] = config;
        }
    }
}