using System;
using _Project.Scripts.Enemies;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Database.Enemies
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public AssetReferenceGameObject prefab;
    }
}