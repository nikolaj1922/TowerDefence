using System;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using UnityEngine.Purchasing;
using Zenject;

namespace _Project.Scripts.UI.Modals.ShopModal
{
    public class ShopModalPresenter: IInitializable, IDisposable
    {
        private readonly IModalCreatorService _modalCreatorService;
        private readonly ShopModalView _view;
        private readonly ISaveLoad _saveLoad;
        
        public ShopModalPresenter(
            ISaveLoad saveLoad,
            ShopModalView view,
            IModalCreatorService modalCreatorService)
        {
            _saveLoad = saveLoad;
            _view = view;
            _modalCreatorService = modalCreatorService;
        }

        public void Initialize()
        {
            _view.OnBackToMainMenuClicked += OnBackToMainMenuClick;
            _view.GetAipListener().onPurchaseCompleteLegacy.AddListener(OnPurchaseCompleted);

            TryToHideNoAdsButton();
        }

       

        public void Dispose()
        {
            _view.OnBackToMainMenuClicked -= OnBackToMainMenuClick;
            _view.GetAipListener().onPurchaseCompleteLegacy.RemoveListener(OnPurchaseCompleted);
        }
        
        private void TryToHideNoAdsButton()
        {
            if (_saveLoad.PlayerProgress.ShowAds == false)
                _view.HideNoAdsButton();
        }
        
        private void OnBackToMainMenuClick() => _modalCreatorService.OpenModal(ModalType.Menu);  
        
        private void OnPurchaseCompleted(Product product)
        {
            switch (product.definition.id)
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
            _view.HideNoAdsButton();
        }

        private void AddCoins(int coins)
        {
            _saveLoad.PlayerProgress.AddMetaCoins(coins);
            _saveLoad.SaveProgress();
        }
    }
}