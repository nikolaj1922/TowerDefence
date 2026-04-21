using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Modals
{
    [CreateAssetMenu(menuName = "Game/Modals Database")]
    public class ModalsPrefabsDatabase : ScriptableObject
    {
        [SerializeField] private List<ModalEntry> _modals;
        
        private Dictionary<ModalType, AssetReferenceGameObject> _map;

        public void Init()
        {
            _map = new Dictionary<ModalType, AssetReferenceGameObject>();

            foreach (var entry in _modals)
                _map[entry.type] = entry.prefab;
        }

        public AssetReferenceGameObject Get(ModalType type)
        {
            if (!_map.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"[ModalsPrefabDatabase] {type} not found");
                return null;
            }

            return prefab;
        }
    }

    public enum ModalType
    {
        Menu,
        Upgrades,
        EndGame
    }
}