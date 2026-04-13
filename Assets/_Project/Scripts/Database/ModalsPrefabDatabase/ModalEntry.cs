using System;
using UnityEngine;

namespace _Project.Scripts.Database.ModalsPrefabDatabase
{
    [Serializable]
    public class ModalEntry
    {
        public ModalType type;
        public GameObject prefab;
    }
}