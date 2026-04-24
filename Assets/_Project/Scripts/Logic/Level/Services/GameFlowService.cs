using System;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Ads;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Logic.Level.Services
{
    public class GameFlowService : IInitializable, IDisposable, IGameFlowService
    {
        private readonly IWaveManager _waveManager;
        private readonly ILevelAnalyticsService _levelAnalyticsService;
        private readonly ILevelUIService _levelUIService;
        private readonly IAdsService _adsService;
        private readonly ICastleService _castleService;

        private bool _canShowAdsModalForSession = true;

        public GameFlowService(
            ICastleService castleService,
            IAdsService adsService,
            ILevelUIService levelUIService,
            IWaveManager waveManager,
            ILevelAnalyticsService levelAnalyticsService
        )
        {
            _castleService = castleService;
            _adsService = adsService;
            _levelUIService = levelUIService;
            _waveManager = waveManager;
            _levelAnalyticsService = levelAnalyticsService;
        }
        
        public void Initialize() => _adsService.OnRewardedAdWatched += RestartWaveOnRewardedAdsWatched;

        public void Dispose() => _adsService.OnRewardedAdWatched -= RestartWaveOnRewardedAdsWatched;

        public void StartLevel() => _waveManager.StartTimer(waveCount: 1);

        public void OnVictory() => _levelUIService.ShowEndModal("Victory!").Forget();

        public void OnDefeat(int towersBuilt)
        {
            _levelAnalyticsService.GameOver(towersBuilt);
            _waveManager.StopWave();

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

        private void RestartWaveOnRewardedAdsWatched()
        {
            _waveManager.StartWave();
            _castleService.Restore();
        }
        
        private bool CanShowAdsModal() => _canShowAdsModalForSession && _adsService.RewardedAd.IsAdReady();
    }
}