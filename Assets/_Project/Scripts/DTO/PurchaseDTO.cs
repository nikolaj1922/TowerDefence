using System;
using UnityEditor.Purchasing;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class PurchaseDTO
    {
        public CatalogPopupProductType type;
        public string id;
        public string title;
        public float price;
    }
}