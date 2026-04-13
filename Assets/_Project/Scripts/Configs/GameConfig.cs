using System;
using UnityEngine;
using System.Collections.Generic;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig",  menuName = "Configs/Level")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public float CastleHealth { get; private set; }
        [field: SerializeField] public Vector3 CastlePosition { get; private set; }
        [field: SerializeField] public int CoinsPerWave { get; private set; }
        [field: SerializeField] public int CoinsPerKill { get; private set; }
        [field: SerializeField] public Wave[] Waves { get; private set; }
        [field: SerializeField] public int TimeBetweenWaves { get; private set; }
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