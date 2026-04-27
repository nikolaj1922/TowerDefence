using System;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class WaveDTO
    {
        public WaveEnemyData[] enemyGroups;
        public float spawnFrequency;
    }
    
    [Serializable]
    public class WaveEnemyData
    {
        public EnemyType enemyType;
        public int enemyCount;
    }
}