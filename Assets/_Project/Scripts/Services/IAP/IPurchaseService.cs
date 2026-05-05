using UnityEngine.Purchasing;

namespace _Project.Scripts.Services.IAP
{
    public interface IPurchaseService
    {
        void OnPurchaseCompleted(string productId);
    }
}