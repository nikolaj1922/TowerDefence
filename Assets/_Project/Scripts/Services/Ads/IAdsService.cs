using System;

namespace _Project.Scripts.Services.Ads
{
    public interface IAdsService
    {
        bool IsRewardedAdReady();
        event Action OnInterstitialAdWatched;
        void ShowRewardedAd(Action onRewardedAdClosed);
        void ShowInterstitialAd();
        bool CanShowInterstitialAdOnTransitionToMenu(int transitionCount);
    }
}