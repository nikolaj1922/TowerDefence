using System;
using _Project.Scripts.Enemy;
using UnityEngine;

namespace _Project.Scripts.Database.EnemyDatabase
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public GameObject prefab;
    }
}