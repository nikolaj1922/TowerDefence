using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Weapon;

namespace _Project.Scripts.Database.WeaponPrefabDatabase
{
    [CreateAssetMenu(menuName = "Game/Weapon Database")]
    public class WeaponPrefabsDatabase : ScriptableObject
    {
        [SerializeField] private List<WeaponEntry> _weapons;

        private Dictionary<WeaponType, Weapon.Weapon> _map;

        public void Init()
        {
            _map = new Dictionary<WeaponType, Weapon.Weapon>();

            foreach (var entry in _weapons)
                _map[entry.type] = entry.prefab;
        }

        public Weapon.Weapon Get(WeaponType type)
        {
            if (!_map.TryGetValue(type, out var prefab))
            {
                Debug.LogError($"WeaponDatabase: prefab for type {type} not found!");
                return null;
            }
            
            return prefab;
        }
    }
}