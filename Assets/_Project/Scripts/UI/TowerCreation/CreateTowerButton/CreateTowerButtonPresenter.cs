using System;
using UnityEngine;
using _Project.Scripts.Towers;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.UI.TowerCreation.CreateTowerButton
{
    public class CreateTowerButtonPresenter: IDisposable
    {
        public event Action OnSuccessCreateTower;
        
        private readonly IWaveManager _waveManager;
        private readonly ITowerService _towerService;
        private readonly ITowerUpgradeService _towerUpgradeService;
        private readonly IAnalyticsService _analyticsService;
        private readonly CoinCounterModel _coinCounterModel;

        private readonly CreateTowerButtonView _view;
        private TowerType _towerType;
        private int _price;
        private readonly Func<Vector3> _getPosition;

        public CreateTowerButtonPresenter(
            IWaveManager waveManager, 
            ITowerService towerService, 
            ITowerUpgradeService towerUpgradeService, 
            IAnalyticsService analyticsService, 
            CoinCounterModel coinCounterModel,
            Func<Vector3> getPosition,
            CreateTowerButtonView view)
        {
            _waveManager = waveManager;
            _towerService = towerService;
            _towerUpgradeService = towerUpgradeService;
            _analyticsService = analyticsService;
            _coinCounterModel = coinCounterModel;

            _getPosition = getPosition;
            _view = view;
        }

        public void Initialize(TowerType towerType, int price)
        {
            _towerType = towerType;
            _price = price;
            
            _view.OnCreateTower += CreateTower;
            _coinCounterModel.OnCoinChanged += OnCoinsChanged;

            OnCoinsChanged(_coinCounterModel.Coins);
        }

        public void Dispose()
        {
            _view.OnCreateTower -= CreateTower;
            _coinCounterModel.OnCoinChanged -= OnCoinsChanged;
        }
        
        private void OnCoinsChanged(int coins) => _view.Draw(coins, _price);

        private void CreateTower(int _)
        {
            if (_coinCounterModel.Coins < _price)
            {
                _analyticsService.BuildRejected("not_enough_coins", _waveManager.CurrentWave);
                return;
            }   
            
            _towerService.CreateAndPurchase(
                _towerType, 
                _getPosition(), 
                _price,
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.TOWER_DAMAGE_ID),
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.TOWER_ATTACK_SPEED_ID)
            );
            
            OnSuccessCreateTower?.Invoke();
        }
    }
}