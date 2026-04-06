using UnityEngine;
using System.Collections.Generic;

namespace _Project.Scripts.Database.ModalsPrefabDatabase
{
    [CreateAssetMenu(menuName = "Game/Modals Database")]
    public class ModalsPrefabDatabase : ScriptableObject
    {
        [SerializeField] private List<ModalEntry> _modals;
        
        private Dictionary<ModalType, GameObject> _map;

        public void Init()
        {
            _map = new Dictionary<ModalType, GameObject>();

            foreach (var entry in _modals)
                _map[entry.type] = entry.prefab;
        }

        public GameObject Get(ModalType type)
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