using System;
using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Enemy;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig",  menuName = "Configs/Level")]
    public class LevelConfig : ScriptableObject
    {
        public float castleHealth;
        public Vector3 castlePosition;
        public Way[] ways;
    }

    [Serializable]
    public class Way
    {
        public List<WayEnemyData> enemies = new();
        public float spawnFrequency;
    }
    
    [Serializable]
    public class WayEnemyData
    {
        public EnemyType enemyType;
        public int enemyCount;
    }
}