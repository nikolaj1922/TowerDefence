using System;
using System.Collections.Generic;
using _Project.Scripts.Database.Purchases;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.SaveLoad;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Services.IAP
{
    public class IAPProvider : IIAPProvider
    {
        private Action _pendingPurchase;
        
        private StoreController _storeController;
        private readonly PurchaseDatabase _purchaseDatabase;
        private readonly ISaveLoad _saveLoad;
        
        public IAPProvider(
            PurchaseDatabase purchaseDatabase, 
            ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _purchaseDatabase = purchaseDatabase;
        }
        
        public void Initialize()
        {
            CatalogProvider catalogProvider = CreateCatalog();
            InitStoreController();
            
            _storeController.Connect().ContinueWith(_ =>
            {
                catalogProvider.FetchProducts(
                    list => _storeController.FetchProducts(list)
                    );
            });
        }

        public void StartPurchase(string productId, Action onPurchaseSuccess = null)
        {
            _pendingPurchase = onPurchaseSuccess;
            _storeController.PurchaseProduct(productId);
        }

        private void InitStoreController()
        {
            _storeController = UnityIAPServices.StoreController();
            _storeController.OnPurchasePending += OnPurchasePending;
            _storeController.OnStoreDisconnected += OnStoreDisconnected;
            _storeController.OnProductsFetched += OnProductsFetched;
            _storeController.OnProductsFetchFailed += OnProductsFetchFailed;
            _storeController.OnPurchasesFetched += OnPurchasesFetched;
            _storeController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
        }

        private CatalogProvider CreateCatalog()
        {
            var catalogProvider = new CatalogProvider();

            foreach (var dto in _purchaseDatabase.GetPurchases())
                catalogProvider.AddProduct(dto.id, (ProductType)dto.type);
            
            return catalogProvider;
        }

        private void OnStoreDisconnected(StoreConnectionFailureDescription failure)
        {
            Debug.LogError("Store disconnected");
        }

        private void OnProductsFetched(List<Product> products)
        {
            _storeController.FetchPurchases();
        }
        
        private void OnProductsFetchFailed(ProductFetchFailed failure)
        {
            Debug.LogError("Products fetch failed");
        }
        
        private void OnPurchasesFetched(Orders orders)
        {
            Debug.Log("Purchase fetched successfully");
        }
        
        private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
        {
            Debug.LogError("Purchase fetch failed");
        }
        
        private void OnPurchasePending(PendingOrder order)
        {
            foreach (var cartItem in order.CartOrdered.Items())
            {
                string productId = cartItem.Product.definition.id;
                Debug.Log("Purchase success: " + productId);
                OnPurchaseCompleted(productId);
            }
            
            _pendingPurchase?.Invoke();
            _pendingPurchase = null;
            _storeController.ConfirmPurchase(order);
        }
        
        private void OnPurchaseCompleted(string productId)
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