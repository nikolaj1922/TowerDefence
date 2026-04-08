using _Project.Scripts.UI;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Logic.Level
{
    public class EndGameService
    {
        private readonly UIFactory _uiFactory;
        private readonly WaveManager _waveManager;
        private readonly ISaveLoad _saveLoad;
        private readonly AnalyticsService _analyticsService;

        public EndGameService(
            UIFactory uiFactory, 
            WaveManager waveManager, 
            ISaveLoad saveLoad,
            AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _saveLoad = saveLoad;
        }

        public void GameOver(int towersBuilt)
        {
            _analyticsService.GameOver(
                _waveManager.CurrentWave, 
                _waveManager.TotalEnemyKilled, 
                towersBuilt, 
                _waveManager.GetRewardForWaves());
            
            EndLevel("Defeat!");
        }
        
        public void GameVictory() => EndLevel("Victory!");
        
        private void EndLevel(string headerText)
        {
            int metaAdded = _waveManager.GetRewardForWaves();
            _saveLoad.AddMetaCoins(metaAdded);
            _uiFactory.CreateEndGameModal(metaAdded, headerText);
        }

    }
}