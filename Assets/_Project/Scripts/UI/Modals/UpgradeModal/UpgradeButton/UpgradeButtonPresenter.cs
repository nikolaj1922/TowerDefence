using System;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.UI.Modals.UpgradeModal.UpgradeButton
{
    public class UpgradeButtonPresenter
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradeButtonView _view;
        private readonly PlayerProgress _progress;
        private readonly UpgradeConfig _upgradeConfig;
        private readonly ITowerUpgradeService _towerUpgradeService;

        private int _price;
        private int _upgradeLevel;
        
        public UpgradeButtonPresenter(
            ISaveLoad saveLoad, 
            UpgradeButtonView view, 
            ITowerUpgradeService towerUpgradeService, 
            UpgradeConfig upgradeConfig)
        {
            _upgradeConfig = upgradeConfig;
            _view = view;
            _saveLoad = saveLoad;
            _progress = saveLoad.PlayerProgress;
            _towerUpgradeService = towerUpgradeService;
        }

        public void Initialize()
        {
            _view.OnBuyClicked += OnBuyClicked;
            Draw();
        }

        public void Dispose() => _view.OnBuyClicked -= OnBuyClicked;

        public void Draw()
        {
            _upgradeLevel = _towerUpgradeService.GetUpgradeLevel(_upgradeConfig.id);
            _price = (int)GetPrice(_upgradeConfig.basePrice, _upgradeConfig.priceMultiplierByLevel, _upgradeLevel);
            
            bool canBuy = _progress.MetaCoinsCount >= _price;
            
            _view.SetTitle(_upgradeConfig.title);
            _view.SetIcon(_upgradeConfig.previewIcon);
            _view.SetDescription(GetDescription());
            _view.SetPrice(_price.ToString(), canBuy ?  Color.cornflowerBlue : Color.red);
            _view.SetInteractable(canBuy);
        }
        
        private void OnBuyClicked()
        {
            _towerUpgradeService.SetUpgradeLevel(_upgradeConfig.id, _upgradeLevel + 1);
            _progress.MetaCoinsCount -= _price;
            _saveLoad.SaveProgress();
        }
        
        private double GetPrice(double basePrice, double multiplier, int level) => 
            basePrice * Math.Pow(multiplier, level - 1);

        private string GetDescription()
            => $"{_upgradeConfig.description} (+{_upgradeConfig.statMultiplierByLevel * _upgradeLevel * 100}%)";
    }
}