using System;
using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Enemy;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig",  menuName = "Configs/Level")]
    public class GameConfig : ScriptableObject
    {
        public float castleHealth;
        public Vector3 castlePosition;

        public int coinsPerWave;
        public int coinsPerKill;
        
        public Wave[] waves;
    }

    [Serializable]
    public class Wave
    {
        public List<WaveEnemyData> enemyGroups = new();
        public float spawnFrequency;
    }
    
    [Serializable]
    public class WaveEnemyData
    {
        public EnemyType enemyType;
        public int enemyCount;
    }
}