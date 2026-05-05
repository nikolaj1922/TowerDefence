using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Ads;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.SaveLoad;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Logic.Level.Services
{
    public class GameFlowService : IGameFlowService
    {
        private readonly ISaveLoad _saveLoad;
        private readonly IWaveManager _waveManager;
        private readonly IAnalyticsService _analyticsService;
        private readonly ILevelUIService _levelUIService;
        private readonly IAdsService _adsService;
        private readonly ICastleService _castleService;
        private readonly IRewardService _rewardService;
        
        private bool _canShowAdsModalForSession = true;
        private bool _canRestartWave;

        public GameFlowService(
            ISaveLoad saveLoad,
            ICastleService castleService,
            IAdsService adsService,
            ILevelUIService levelUIService,
            IWaveManager waveManager,
            IAnalyticsService analyticsService,
            IRewardService rewardService
        )
        {
            _saveLoad = saveLoad;
            _rewardService = rewardService;
            _analyticsService = analyticsService;
            _castleService = castleService;
            _adsService = adsService;
            _levelUIService = levelUIService;
            _waveManager = waveManager;

            _canRestartWave = !saveLoad.PlayerProgress.ShowAds;
        }

        public void StartLevel() => _waveManager.StartTimer(waveCount: 1);

        public void OnVictory() => _levelUIService.ShowEndModal("Victory!").Forget();

        public void OnDefeat(int towersBuilt)
        {
            _analyticsService.GameOver(
                _waveManager.CurrentWave,
                _waveManager.TotalEnemyKilled,
                towersBuilt,
                _rewardService.GetReward());
            
            _waveManager.StopWave();

            if (_canRestartWave)
            {
                _canRestartWave = false;
                RestartWave();
                return;
            }

            if (CanShowAdsModal())
            {
                _canShowAdsModalForSession = false;
                _levelUIService.ShowContinueForAdsModal();
            }
            else
            {
                _levelUIService.ShowEndModal("Defeat!").Forget();
            }
        }

        private void RestartWave()
        {
            _waveManager.StartWave();
            _castleService.Restore();
        }

        private bool CanShowAdsModal()
        {
            return _saveLoad.PlayerProgress.ShowAds && _canShowAdsModalForSession && _adsService.IsRewardedAdReady();
        }
    }
}