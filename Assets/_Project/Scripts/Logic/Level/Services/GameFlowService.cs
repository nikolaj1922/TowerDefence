using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.SaveLoad;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Logic.Level.Services
{
    public class GameFlowService : IGameFlowService
    {
        private readonly IWaveManager _waveManager;
        private readonly ILevelAnalyticsService _levelAnalyticsService;
        private readonly ISaveLoad _saveLoad;
        private readonly ILevelUIService _levelUIService;
        
        public GameFlowService(
            ILevelUIService levelUIService,
            IWaveManager waveManager,
            ILevelAnalyticsService levelAnalyticsService,
            ISaveLoad saveLoad
         )
        {
            _levelUIService = levelUIService;
            _waveManager = waveManager;
            _levelAnalyticsService = levelAnalyticsService;
            _saveLoad = saveLoad;
        }
        
        public void StartLevel() => _waveManager.StartTimer(waveCount: 1);

        public void OnVictory()
        {
            SaveReward();
            _levelUIService.ShowEndModal("Victory!").Forget();
        }

        public void OnDefeat(int towersBuilt)
        {
            _levelAnalyticsService.GameOver(towersBuilt);
            _waveManager.StopWave();
            SaveReward();
            _levelUIService.ShowEndModal("Defeat!").Forget();
        }

        private void SaveReward()
        {
            _saveLoad.PlayerProgress.AddMetaCoins(_waveManager.GetReward());
            _saveLoad.SaveProgress();
        }
    }
}