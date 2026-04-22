using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.UI.CoinCounter;

namespace _Project.Scripts.Logic.Level.Services
{
    public class LevelAnalyticsService : ILevelAnalyticsService
    {
        private readonly IAnalyticsService _analytics;
        private readonly IWaveManager _waveManager;

        private readonly CoinCounterModel _coinCounter;
        
        public LevelAnalyticsService(
            IAnalyticsService analytics, 
            IWaveManager waveManager,
            CoinCounterModel coinCounter
            )
        {
            _coinCounter = coinCounter;
            _analytics = analytics;
            _waveManager = waveManager;
        }

        public void OnTowerBuilt(int coinPrice, int totalTowers) =>
            _analytics.TowerBuilt(_waveManager.CurrentWave, coinPrice, _coinCounter.Coins, totalTowers);

        public void OnWaveCompleted(int wave, int towersBuilt) =>
            _analytics.WaveCompleted(wave, towersBuilt, _coinCounter.Coins);

        public void OnCastleDamaged(float hp) =>
            _analytics.CastleDamaged(_waveManager.CurrentWave, hp);

        public void GameOver(int towersBuilt) =>  _analytics.GameOver(
            _waveManager.CurrentWave, 
            _waveManager.TotalEnemyKilled, 
            towersBuilt, 
            _waveManager.GetReward());
    }
}