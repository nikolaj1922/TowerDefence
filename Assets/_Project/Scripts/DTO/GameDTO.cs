using System;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class GameDTO
    {
        public float castleHealth;
        public int castlePositionX;
        public int castlePositionY;
        public int castlePositionZ;
        public int coinsPerWave;
        public int coinsPerKill;
        public int timeBetweenWaves;
    }
}