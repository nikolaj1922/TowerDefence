using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using _Project.Scripts.Services.RemoteConfigs;

namespace _Project.Scripts.Database.Weapons
{
    [CreateAssetMenu(menuName = "Game/Weapon Database")]
    public class WeaponDatabase : ScriptableObject, IPrefabDatabase
    {
        [SerializeField] private WeaponEntry[] _weaponAssets;

        private readonly DatabasePrefabLoader<WeaponType, Weapon> _prefabLoader = new();
        private readonly Dictionary<WeaponType, WeaponDTO> _configs = new();

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
        
        public WeaponDTO GetConfig(WeaponType type)
        {
            if (_configs == null)
            {
                Debug.LogError("Configs not initialized!");
                return null;
            }
            
            if (!_configs.TryGetValue(type, out var config))
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

        public void LoadConfig(IRemoteConfigService remoteConfigService)
        {
            if (!remoteConfigService.TryGetConfig<RemoteConfig<WeaponDTO>>(GameConstants.WEAPON_REMOTE_CONFIG_KEY, out var config))
            {
                Debug.LogError("Loading weapon configs failed");
                return;
            }
            
            foreach (var dto in config.items)
                _configs[dto.type] = dto;
        }

        public UniTask UnloadPrefabs()
        {
            _prefabLoader.UnloadAssets();
            return UniTask.CompletedTask;
        }
    }
}