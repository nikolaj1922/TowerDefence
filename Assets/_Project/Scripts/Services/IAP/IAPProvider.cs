using System.Collections.Generic;
using _Project.Scripts.Database.Purchases;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Services.IAP
{
    public class IAPProvider : IIAPProvider
    {
        private StoreController _storeController;
        private readonly PurchaseDatabase _purchaseDatabase;
        private readonly IPurchaseService _purchaseService;
        private readonly Dictionary<string, Product> _products = new();
        
        public IAPProvider(PurchaseDatabase purchaseDatabase, IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
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

        public void StartPurchase(string productId) => _storeController.PurchaseProduct(productId);

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
            Debug.Log("Products fetched successfully");
            
            foreach (var product in  products)
                _products[product.definition.id] = product;
            
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
                _purchaseService.OnPurchaseCompleted(productId);
            }

            _storeController.ConfirmPurchase(order);
        }
    }
}