using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.GameSession;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.Towers;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerPanelPresenter: IDisposable
    {
        private readonly IInstantiator _instantiator;
        private readonly CoinCounterModel _coinCounterModel;
        private readonly ITowerService _towerService;
        private readonly IWaveManager _waveManager;
        private readonly IAnalyticsService _analyticsService;
        private readonly ITowerUpgradeService _towerUpgradeService;
        private readonly IGameSessionService _gameSessionService;

        private readonly CreateTowerPanelView _view;
        private readonly CreateTowerPanelModel _model;

        public CreateTowerPanelPresenter(
            IInstantiator instantiator,
            CoinCounterModel coinCounterModel, 
            IAnalyticsService analyticsService, 
            ITowerService towerService, 
            IWaveManager waveManager,
            ITowerUpgradeService towerUpgradeService,
            IGameSessionService gameSessionService,
            CreateTowerPanelView view,
            CreateTowerPanelModel model
            )
        {
            _instantiator =  instantiator;
            _model = model;
            _view = view;
            _gameSessionService = gameSessionService;
            _towerUpgradeService = towerUpgradeService;
            _analyticsService = analyticsService;
            _coinCounterModel = coinCounterModel;
            _towerService = towerService;
            _waveManager = waveManager;
        }

        public void Initialize()
        {
            foreach (TowerDTO towerConfig in _model.BuildableTowerConfigs)
                DrawTowerButtons(towerConfig);
            
            _coinCounterModel.UpdateSubscribers();
        }

        public void Dispose()
        {
            foreach (CreateTowerButtonView towerButton in _model.TowerButtons)
            {
                towerButton.OnCreateTower -= CreateTower;
                _coinCounterModel.OnCoinChanged -= towerButton.Draw;
            }
            
            _model.ClearButtons();
        }

        public void ShowTowerPanel(Vector3 pos)
        {
            _model.SetTowerPosition(pos);
            _view.ShowPanel();
        }

        public void HidePanel() => _view.HidePanel();

        private void DrawTowerButtons(TowerDTO towerDto)
        {
            CreateTowerButtonView towerButtonView =
                _instantiator.InstantiatePrefabForComponent<CreateTowerButtonView>(
                    _view.CreateTowerButtonView, 
                    _view.PanelRectTransform
                );
            
            towerButtonView.Initialize(towerDto.iconAddress, towerDto.coinPrice, towerDto.type);
            towerButtonView.OnCreateTower += CreateTower;
            _coinCounterModel.OnCoinChanged += towerButtonView.Draw;
            
            _model.RegisterButton(towerButtonView);
        }
        
        private void CreateTower(int price, TowerType towerType)
        {
            if (_coinCounterModel.Coins < price)
            {
                _analyticsService.BuildRejected("not_enough_coins", _waveManager.CurrentWave);
                return;
            }
            
            _towerService.CreateAndPurchase(
                towerType, 
                _model.TowerPosition, 
                price,
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.TOWER_DAMAGE_ID),
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.TOWER_ATTACK_SPEED_ID)
            );

            _gameSessionService.BuildTowerOnLevel();
            HidePanel();
            
            _analyticsService.TowerBuilt(_waveManager.CurrentWave, price, _coinCounterModel.Coins, _gameSessionService.TowerBuiltOnLevel);
        }
    }
}