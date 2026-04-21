using System;
using _Project.Scripts.Towers;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Towers
{
    [Serializable]
    public class TowerEntry
    {
        public TowerType type;
        public AssetReferenceGameObject prefab;
    }
}