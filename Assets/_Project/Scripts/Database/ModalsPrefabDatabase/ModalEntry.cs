using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.ModalsPrefabDatabase
{
    [Serializable]
    public class ModalEntry
    {
        public ModalType type;
        public AssetReferenceGameObject prefab;
    }
}