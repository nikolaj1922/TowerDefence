using System;
using _Project.Scripts.Weapons;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Weapons
{
    [Serializable]
    public class WeaponEntry
    {
        public WeaponType type;
        public AssetReferenceGameObject prefab;
    }
}