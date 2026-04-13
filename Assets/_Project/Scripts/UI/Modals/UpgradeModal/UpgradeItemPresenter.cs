using System;
using UnityEngine;
using _Project.Scripts.Configs.Upgrades;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.UI.Modals.UpgradeModal
{
    public class UpgradeItemPresenter :IDisposable
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradeItemView _view;
        private readonly PlayerProgress _progress;
        private readonly UpgradeConfig _upgradeConfig;
        private readonly TowerUpgradeService _towerUpgradeService;

        private int _price;
        private int _upgradeLevel;
        
        public UpgradeItemPresenter(
            ISaveLoad saveLoad, 
            UpgradeItemView view, 
            UpgradeConfig upgradeConfig,
            TowerUpgradeService towerUpgradeService
            )
        {
            _view = view;
            _saveLoad = saveLoad;
            _progress = saveLoad.PlayerProgress;
            _upgradeConfig = upgradeConfig;
            _towerUpgradeService = towerUpgradeService;

            Init();
        }
        
        public void Dispose() => _view.OnBuyClicked -= OnBuyClicked;

        private void Init()
        {
            Refresh();
            
            _view.OnBuyClicked += OnBuyClicked;
            _view.SetTitle(_upgradeConfig.title);
            _view.SetIcon(_upgradeConfig.previewIcon);
        }
        
        private void OnBuyClicked()
        {
            _towerUpgradeService.SetUpgradeLevel(_upgradeConfig.id, _upgradeLevel + 1);
            _progress.metaCoinsCount -= _price;
            _saveLoad.SaveProgress();
            Refresh();
        }

        private void Refresh()
        {
            _upgradeLevel = _towerUpgradeService.GetUpgradeLevel(_upgradeConfig.id);
            _price = (int)GetPrice(_upgradeConfig.basePrice, _upgradeConfig.priceMultiplierByLevel, _upgradeLevel);
            
            bool canBuy = _progress.metaCoinsCount >= _price;
            
            _view.SetMetaIconActive(true);
            _view.SetDescription(GetDescription());
            _view.SetPrice(_price.ToString(), canBuy ?  Color.cornflowerBlue : Color.red);
            _view.SetInteractable(canBuy);
        }
        
        private double GetPrice(double basePrice, double multiplier, int level) => 
            basePrice * Math.Pow(multiplier, level - 1);

        private string GetDescription()
            => $"{_upgradeConfig.description} (+{_upgradeConfig.statMultiplierByLevel * _upgradeLevel * 100}%)";
    }
}