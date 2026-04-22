using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace _Project.Scripts.Database.Weapons
{
    [CreateAssetMenu(menuName = "Game/Weapon Database")]
    public class WeaponDatabase : ScriptableObject, IPrefabDatabase, IConfigDatabase
    {
        [SerializeField] private WeaponEntry[] _weaponAssets;

        private readonly DatabasePrefabLoader<WeaponType, Weapon> _prefabLoader = new();
        private readonly DatabaseConfigLoader<WeaponType, WeaponConfig> _configLoader = new();

        public Weapon Get(WeaponType type)
        {
            if (_prefabLoader.Prefabs == null)
            {
                Debug.LogError("Prefabs not initialized!");
                return null;
            }
            
            if (!_prefabLoader.Prefabs.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"Prefab not found for type: {type}");
                return null;
            }
            
            return prefab;
        }
        
        public WeaponConfig GetConfig(WeaponType type)
        {
            if (_configLoader.Configs == null)
            {
                Debug.LogError("Configs not initialized!");
                return null;
            }
            
            if (!_configLoader.Configs.TryGetValue(type, out var config))
            {
                Debug.LogError($"Config not found for type: {type}");
                return null;
            }

            return config;
        }
        
        public async UniTask LoadPrefabs()
        {
            await _prefabLoader.LoadAssets(
                _weaponAssets.Select(x => (x.type, x.prefab)),
                go => go.GetComponent<Weapon>()
            );
        }

        public async UniTask LoadConfigs()
        {
            await _configLoader.LoadAssets(
                GameConstants.WEAPON_CONFIG_ASSET_LABEL,
                (x) => x.WeaponType);
        }

        public UniTask UnloadPrefabs()
        {
            _prefabLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }

        public UniTask UnloadConfigs()
        {
            _configLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}