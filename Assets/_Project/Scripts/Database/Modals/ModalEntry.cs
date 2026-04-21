using System;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Modals
{
    [Serializable]
    public class ModalEntry
    {
        public ModalType type;
        public AssetReferenceGameObject prefab;
    }
}