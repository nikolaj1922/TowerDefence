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
        
        public LevelPlayRewardedAd RewardedAd { get;  private set; }
        public LevelPlayInterstitialAd InterstitialAd { get; private set; }
        
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

            if (RewardedAd != null)
            {
                RewardedAd.OnAdClosed -= RewardedOnAdClosedEvent;
                RewardedAd.OnAdRewarded -= RewardedOnAdRewardedEvent;
            }

            if (InterstitialAd != null)
            {
                InterstitialAd.OnAdClosed -= InterstitialOnAdClosedEvent;
            }
        }
        
        public void ShowRewardedAd(Action onReward)
        {
            if (!RewardedAd.IsAdReady())
                return;

            _pendingReward = onReward;
            RewardedAd.ShowAd();
        }
        
        public void ShowInterstitialAd()
        {
            if (InterstitialAd.IsAdReady())
                InterstitialAd.ShowAd();
        }
        
        public bool CanShowInterstitialAdOnTransitionToMenu(int transitionCount) =>
            InterstitialAd.IsAdReady() && transitionCount % 2 == 0;

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
            InterstitialAd = new LevelPlayInterstitialAd(GameConstants.INTERSTITIAL_ADS_ID);

            InterstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        }
        
        private void CreateRewardedAd() {
            RewardedAd = new LevelPlayRewardedAd(GameConstants.REWARDED_ADS_ID_RESTORE_CASTLE_HP);
            
            RewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
            RewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        }
        
        private void LoadRewardedAd() => RewardedAd.LoadAd();
        
        private void LoadInterstitialAd() => InterstitialAd.LoadAd();

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