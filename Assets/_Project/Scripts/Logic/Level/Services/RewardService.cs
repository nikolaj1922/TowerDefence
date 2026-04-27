using _Project.Scripts.Database.Game;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;

namespace _Project.Scripts.Logic.Level.Services
{
    public class RewardService : IRewardService
    {
        private readonly GameDatabase _gameDatabase;
        private readonly IWaveManager _waveManager;

        public RewardService(GameDatabase gameDatabase, IWaveManager waveManager)
        {
            _gameDatabase = gameDatabase;
            _waveManager = waveManager;
        }
        
        public int GetReward() =>
            (_waveManager.CurrentWave) * _gameDatabase.GetConfig().coinsPerWave
            + _waveManager.TotalEnemyKilled * _gameDatabase.GetConfig().coinsPerKill;
    }
}