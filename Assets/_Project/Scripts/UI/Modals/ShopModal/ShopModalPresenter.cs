using System;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Database.Purchases;
using _Project.Scripts.DTO;
using _Project.Scripts.Services.IAP;
using _Project.Scripts.Services.ModalCreator;
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

        public ShopModalPresenter(
            PurchaseDatabase purchaseDatabase,
            ShopModalView view,
            ShopModalButtonView shopButtonView,
            IInstantiator instantiator,
            IIAPProvider iapProvider,
            IModalCreatorService modalCreatorService)
        {
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
            
            shopButtonView.Init(
                purchaseDto.title,
                $"{purchaseDto.price}$", 
                () => _iapProvider.StartPurchase(purchaseDto.id));
        } 
        
        private void OnBackToMainMenuClick() => _modalCreatorService.OpenModal(ModalType.Menu);  
        
        
    }
}