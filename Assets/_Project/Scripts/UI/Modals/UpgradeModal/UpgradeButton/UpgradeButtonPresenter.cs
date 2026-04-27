using System;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.TowerUpgrade;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.UI.Modals.UpgradeModal.UpgradeButton
{
    public class UpgradeButtonPresenter
    {
        private readonly ISaveLoad _saveLoad;
        private readonly IAssetProviderService _assetProviderService;
        private readonly UpgradeButtonView _view;
        private readonly PlayerProgress _progress;
        private readonly UpgradeDTO _upgradeDto;
        private readonly ITowerUpgradeService _towerUpgradeService;

        private int _price;
        private int _upgradeLevel;
        
        public UpgradeButtonPresenter(
            ISaveLoad saveLoad, 
            IAssetProviderService assetProviderService,
            UpgradeButtonView view, 
            ITowerUpgradeService towerUpgradeService, 
            UpgradeDTO upgradeDto)
        {
            _upgradeDto = upgradeDto;
            _assetProviderService = assetProviderService;
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

        public void Dispose()
        {
            _view.OnBuyClicked -= OnBuyClicked;
            _assetProviderService.ReleaseByAddress(_upgradeDto.iconAddress);
        }

        public void Draw()
        {
            _upgradeLevel = _towerUpgradeService.GetUpgradeLevel(_upgradeDto.id);
            _price = (int)GetPrice(_upgradeDto.basePrice, _upgradeDto.priceMultiplierByLevel, _upgradeLevel);
            
            bool canBuy = _progress.MetaCoinsCount >= _price;
            
            _view.SetTitle(_upgradeDto.title);
            _view.SetDescription(GetDescription());
            _view.SetPrice(_price.ToString(), canBuy ?  Color.cornflowerBlue : Color.red);
            _view.SetInteractable(canBuy);
            LoadPreview(_upgradeDto.iconAddress).Forget();
        }
        
        private async UniTask LoadPreview(string iconAddress)
        {
            Sprite previewSprite = await _assetProviderService.LoadByAddress<Sprite>(iconAddress);
            _view.SetIcon(previewSprite);
        }
        
        private void OnBuyClicked()
        {
            _towerUpgradeService.SetUpgradeLevel(_upgradeDto.id, _upgradeLevel + 1);
            _progress.SpendMetaCoins(_price);
            _saveLoad.SaveProgress();
        }
        
        private double GetPrice(double basePrice, double multiplier, int level) => 
            basePrice * Math.Pow(multiplier, level - 1);

        private string GetDescription()
            => $"{_upgradeDto.description} (+{_upgradeDto.statMultiplierByLevel * _upgradeLevel * 100}%)";
    }
}