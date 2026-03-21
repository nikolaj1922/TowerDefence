using System;
using _Project.Scripts.Tower;
using UnityEngine;

namespace _Project.Scripts.Database.TowersDatabase
{
    [Serializable]
    public class TowerEntry
    {
        public TowerType type;
        public GameObject prefab;
    }
}