using System;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Upgrades/Upgrade")]
    public class UpgradeConfig: ScriptableObject
    {
        public string id;
        
        public string title;
        public string description;
        public Sprite previewIcon;

        public int basePrice;
        public int priceMultiplierByLevel;

        public float statMultiplierByLevel;
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = Guid.NewGuid().ToString();
        }
    }
}