using System;
using _Project.Scripts.Infrastructure.Constants;
using Unity.Services.LevelPlay;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.Ads
{
    public class AdsService : IInitializable, IAdsService, IDisposable
    {
        private Action _pendingReward;
        public event Action OnInterstitialAdWatched;
        
        private LevelPlayRewardedAd _rewardedAd;
        private LevelPlayInterstitialAd _interstitialAd;
        
        public void Initialize()
        {
            LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
            LevelPlay.Init(GameConstants.LEVEL_PLAY_APP_ID);
        }
        
        public void Dispose()
        {
            LevelPlay.OnInitSuccess -= SdkInitializationCompletedEvent;
            LevelPlay.OnInitFailed -= SdkInitializationFailedEvent;

            if (_rewardedAd != null)
            {
                _rewardedAd.OnAdClosed -= RewardedOnAdClosedEvent;
                _rewardedAd.OnAdRewarded -= RewardedOnAdRewardedEvent;
            }

            if (_interstitialAd != null)
            {
                _interstitialAd.OnAdClosed -= InterstitialOnAdClosedEvent;
            }
        }
        
        public bool IsRewardedAdReady() => _rewardedAd?.IsAdReady() == true;
        
        public void ShowRewardedAd(Action onReward)
        {
            if (!_rewardedAd.IsAdReady())
                return;

            _pendingReward = onReward;
            _rewardedAd.ShowAd();
        }
        
        public void ShowInterstitialAd()
        {
            if (_interstitialAd.IsAdReady())
                _interstitialAd.ShowAd();
        }
        
        public bool CanShowInterstitialAdOnTransitionToMenu(int transitionCount) =>
            _interstitialAd.IsAdReady() && transitionCount % 2 == 0;

        private void SdkInitializationFailedEvent(LevelPlayInitError obj)
        {
            Debug.LogError("Level Play SDK Initialization Failed");
        }

        private void SdkInitializationCompletedEvent(LevelPlayConfiguration obj)
        {
            Debug.Log("Level Play SDK Initialization Success");
            CreateRewardedAd();
            LoadRewardedAd();
            
            CreateInterstitialAd();
            LoadInterstitialAd();
        }
        
        private void CreateInterstitialAd()
        {
            _interstitialAd = new LevelPlayInterstitialAd(GameConstants.INTERSTITIAL_ADS_ID);

            _interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        }
        
        private void CreateRewardedAd() {
            _rewardedAd = new LevelPlayRewardedAd(GameConstants.REWARDED_ADS_ID_RESTORE_CASTLE_HP);
            
            _rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
            _rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        }
        
        private void LoadRewardedAd() => _rewardedAd.LoadAd();
        
        private void LoadInterstitialAd() => _interstitialAd.LoadAd();

        private void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) => LoadRewardedAd();

        private void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
        {
            LoadInterstitialAd();
            OnInterstitialAdWatched?.Invoke();
        }

        private void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward)
        {
            _pendingReward?.Invoke();
            _pendingReward = null;
        }
    }
}