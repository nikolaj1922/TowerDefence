using System;

namespace _Project.Scripts.Services.IAP
{
    public interface IIAPProvider
    {
        void Initialize();
        void StartPurchase(string productId, Action onPurchaseSuccess = null);
    }
}