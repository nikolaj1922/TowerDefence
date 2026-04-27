using System;
using Zenject;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Database.Upgrades;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.UI.Modals.UpgradeModal.UpgradeButton;

namespace _Project.Scripts.UI.Modals.UpgradeModal
{
    public class UpgradeModalPresenter : IInitializable, IDisposable
    {
        private readonly UpgradeModalView _view;
        private readonly UpgradeButtonView _upgradeButtonView;

        private readonly IAssetProviderService _assetProviderService;
        private readonly ISaveLoad _saveLoad;
        private readonly IInstantiator _instantiator;
        private readonly ITowerUpgradeService _towerUpgradeService;
        private readonly IModalCreatorService _modalCreatorService;
        private readonly UpgradeDatabase _upgradeDatabase;

        private List<UpgradeButtonPresenter> _upgradeButtons;
        private List<UpgradeButtonView> _upgradeViews;

        public UpgradeModalPresenter(
            IAssetProviderService assetProviderService,
            IInstantiator instantiator, 
            UpgradeButtonView upgradeButtonView, 
            UpgradeModalView view,
            ISaveLoad saveLoad, 
            IModalCreatorService modalCreatorService, 
            ITowerUpgradeService towerUpgradeService, 
            UpgradeDatabase upgradeDatabase)
        {
            _view = view;
            _instantiator = instantiator;

            _saveLoad = saveLoad;
            _assetProviderService = assetProviderService;
            _towerUpgradeService = towerUpgradeService;
            _upgradeDatabase = upgradeDatabase;
            _modalCreatorService = modalCreatorService;
            _upgradeButtonView = upgradeButtonView;
        }
        
        public void Initialize()
        {
            _view.OnBackToMainMenuClicked += OnBackToMainMenuClick;
            
            _upgradeButtons = new  List<UpgradeButtonPresenter>();
            _upgradeViews = new  List<UpgradeButtonView>();
            
            _view.MetaCounterView.UpdateView(_saveLoad.PlayerProgress.MetaCoinsCount.ToString());
            
            foreach (var upgrade in _upgradeDatabase.GetUpgrades())
                CreateUpgradeButton(upgrade);
        }
        
        public void Dispose()
        {
            _view.OnBackToMainMenuClicked -= OnBackToMainMenuClick;
            
            foreach (UpgradeButtonView upgradeButtonView in _upgradeViews)
                upgradeButtonView.OnBuyClicked -= RefreshListAfterBuy;
            
            foreach (UpgradeButtonPresenter upgradeButton in _upgradeButtons)
                upgradeButton.Dispose();
        }
        
        private void OnBackToMainMenuClick() => _modalCreatorService.OpenModal(ModalType.Menu);  
        
        private void CreateUpgradeButton(UpgradeDTO upgradeDto)
        {
            UpgradeButtonView upgradeView = 
                _instantiator.InstantiatePrefabForComponent<UpgradeButtonView>(
                    _upgradeButtonView, 
                    _view.GridContainer);
            
            UpgradeButtonPresenter upgradeButton = new UpgradeButtonPresenter(
                _saveLoad, 
                _assetProviderService,
                upgradeView, 
                _towerUpgradeService, 
                upgradeDto);
            
            upgradeButton.Initialize();
            upgradeView.OnBuyClicked += RefreshListAfterBuy;
            
            _upgradeButtons.Add(upgradeButton);
            _upgradeViews.Add(upgradeView);
        } 
        
        private void RefreshListAfterBuy()
        {
            _view.MetaCounterView.UpdateView(_saveLoad.PlayerProgress.MetaCoinsCount.ToString());
            
            foreach (UpgradeButtonPresenter upgradeButton in _upgradeButtons)
                upgradeButton.Draw();
        }
    }
}