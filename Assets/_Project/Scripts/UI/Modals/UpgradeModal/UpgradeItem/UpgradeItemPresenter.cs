using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.TowerUpgrade;
using UnityEngine;

namespace _Project.Scripts.UI.Modals.UpgradeModal.UpgradeItem
{
    public class UpgradeItemPresenter :IDisposable
    {
        private readonly ISaveLoad _saveLoad;
        private readonly PlayerProgress _progress;
        private readonly UpgradeConfig _upgradeConfig;
        private readonly TowerUpgradeService _towerUpgradeService;

        private int _price;
        private int _upgradeLevel;

        public UpgradeItemView View { get; private set; }

        public UpgradeItemPresenter(
            ISaveLoad saveLoad, 
            UpgradeItemView view, 
            UpgradeConfig upgradeConfig,
            TowerUpgradeService towerUpgradeService
            )
        {
            View = view;
            _saveLoad = saveLoad;
            _progress = saveLoad.PlayerProgress;
            _upgradeConfig = upgradeConfig;
            _towerUpgradeService = towerUpgradeService;

            Init();
        }

        public void Dispose()
        {
            View.OnBuyClicked -= OnBuyClicked;
        }
        
        public void Refresh()
        {
            _upgradeLevel = _towerUpgradeService.GetUpgradeLevel(_upgradeConfig.id);
            _price = (int)GetPrice(_upgradeConfig.basePrice, _upgradeConfig.priceMultiplierByLevel, _upgradeLevel);
            
            bool canBuy = _progress.metaCoinsCount >= _price;
            
            View.SetMetaIconActive(true);
            View.SetDescription(GetDescription());
            View.SetPrice(_price.ToString(), canBuy ?  Color.cornflowerBlue : Color.red);
            View.SetInteractable(canBuy);
        }

        private void Init()
        {
            Refresh();
            
            View.OnBuyClicked += OnBuyClicked;
            View.SetTitle(_upgradeConfig.title);
            View.SetIcon(_upgradeConfig.previewIcon);
        }
        
        private void OnBuyClicked()
        {
            _towerUpgradeService.SetUpgradeLevel(_upgradeConfig.id, _upgradeLevel + 1);
            _progress.metaCoinsCount -= _price;
            _saveLoad.SaveProgress();
        }
        
        private double GetPrice(double basePrice, double multiplier, int level) => 
            basePrice * Math.Pow(multiplier, level - 1);

        private string GetDescription()
            => $"{_upgradeConfig.description} (+{_upgradeConfig.statMultiplierByLevel * _upgradeLevel * 100}%)";
    }
}