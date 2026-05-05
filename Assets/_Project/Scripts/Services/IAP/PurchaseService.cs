using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Services.IAP
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ISaveLoad _saveLoad;
        
        public PurchaseService(ISaveLoad saveLoad) => _saveLoad = saveLoad;

        public void OnPurchaseCompleted(string productId)
        {
            switch (productId)
            {
                case GameConstants.PURCHASE_ID_500_COINS:
                    AddCoins(500);
                    break;
                case GameConstants.PURCHASE_ID_1500_COINS:
                    AddCoins(1500);
                    break;
                case GameConstants.PURCHASE_ID_4000_COINS:
                    AddCoins(4000);
                    break;
                case GameConstants.PURCHASE_ID_10000_COINS:
                    AddCoins(10000);
                    break;
                case GameConstants.PURCHASE_ID_NO_ADS:
                    RemoveAds();
                    break;
            }
        }

        private void RemoveAds()
        {
            _saveLoad.PlayerProgress.DisableAds();
            _saveLoad.SaveProgress();
        }

        private void AddCoins(int coins)
        {
            _saveLoad.PlayerProgress.AddMetaCoins(coins);
            _saveLoad.SaveProgress();
        }
    }
}