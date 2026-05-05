using System;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Database.Purchases;
using _Project.Scripts.DTO;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.IAP;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using Zenject;

namespace _Project.Scripts.UI.Modals.ShopModal
{
    public class ShopModalPresenter: IInitializable, IDisposable
    {
        private readonly ShopModalView _view;
        private readonly ShopModalButtonView _shopButtonView;
        
        private readonly IModalCreatorService _modalCreatorService;
        private readonly IIAPProvider _iapProvider;
        private readonly PurchaseDatabase _purchaseDatabase;
        private readonly IInstantiator _instantiator;
        private readonly ISaveLoad _saveLoad;
        
        private ShopModalButtonView _noAdsButtonView;

        public ShopModalPresenter(
            PurchaseDatabase purchaseDatabase,
            ShopModalView view,
            ShopModalButtonView shopButtonView,
            ISaveLoad saveLoad,
            IInstantiator instantiator,
            IIAPProvider iapProvider,
            IModalCreatorService modalCreatorService)
        {
            _saveLoad = saveLoad;
            _shopButtonView = shopButtonView;
            _instantiator = instantiator;
            _purchaseDatabase = purchaseDatabase;
            _iapProvider = iapProvider;
            _view = view;
            _modalCreatorService = modalCreatorService;
        }

        public void Initialize()
        {
            _view.OnBackToMainMenuClicked += OnBackToMainMenuClick;
            
            foreach (var dto in _purchaseDatabase.GetPurchases())
                CreateShopButton(dto);

            HideNoAdsButtonIfNeeded();
        }
        
        public void Dispose()
        {
            _view.OnBackToMainMenuClicked -= OnBackToMainMenuClick;
        }
        
        private void CreateShopButton(PurchaseDTO purchaseDto)
        {
            ShopModalButtonView shopButtonView = 
                _instantiator.InstantiatePrefabForComponent<ShopModalButtonView>(
                    _shopButtonView, 
                    _view.GridContainer);

            if (purchaseDto.id == GameConstants.PURCHASE_ID_NO_ADS)
                _noAdsButtonView = shopButtonView;
            
            shopButtonView.Init(
                purchaseDto.title,
                $"{purchaseDto.price}$",
                () => _iapProvider.StartPurchase(purchaseDto.id, onPurchaseSuccess: HideNoAdsButtonIfNeeded));
        } 
        
        private void OnBackToMainMenuClick() => _modalCreatorService.OpenModal(ModalType.Menu);

        private void HideNoAdsButtonIfNeeded()
        {
            if (_noAdsButtonView == null)
                return;

            if (_saveLoad.PlayerProgress.ShowAds == false)
                _noAdsButtonView.gameObject.SetActive(false);
        }
    }
}