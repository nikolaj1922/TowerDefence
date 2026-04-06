using System;
using UnityEngine;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Configs.Upgrades
{
    public abstract class UpgradeBaseConfig: ScriptableObject
    {
        public string id;
        public string title;
        public Sprite previewIcon; 
        public UpgradeLevel[] levels;

        public abstract void OnBuy(PlayerProgress progress, float multiplier);
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = Guid.NewGuid().ToString();
        }
    }
}