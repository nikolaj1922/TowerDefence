using System;
using Unity.Services.LevelPlay;

namespace _Project.Scripts.Services.Ads
{
    public interface IAdsService
    {
        event Action OnRewardedAdWatched;
        event Action OnInterstitialAdWatched;
        LevelPlayRewardedAd RewardedAd { get; }
        void ShowRewardedAd();
        void ShowInterstitialAd();
        bool CanShowInterstitialAdOnTransitionToMenu(int transitionCount);
    }
}