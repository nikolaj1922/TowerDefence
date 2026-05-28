using System;
using UnityEngine.Purchasing;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class PurchaseDTO
    {
        public ProductType type;
        public string id;
        public string title;
        public float price;
    }
}