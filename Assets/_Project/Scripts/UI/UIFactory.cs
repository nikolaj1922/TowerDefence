using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.Database.Towers;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.GameSession;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.Towers;

namespace _Project.Scripts.UI
{
    public class UIFactory: IDisposable
    {
        private Vector3 _createTowerPosition;
        
        private IInstantiator _instantiator;
        private IWaveManager _waveManager;
        private ITowerService _towerService;
        private ITowerUpgradeService _towerUpgradeService;
        private IGameSession _gameSession;
        private CoinCounterView _coinCounterView;
        private WaveCounterView _waveCounterView;
        private CoinCounterModel _coinCounterModel;
        private IAnalyticsService _analyticsService;
        private CreateTowerPanelView _createTowerPanelView;
        private TowerDatabase _towerDatabase;

        private CreateTowerPanelPresenter _towerPanelPresenter;

        [Inject]
        public void Construct(
            IGameSession gameSession,
            IAnalyticsService analyticsService,
            ITowerUpgradeService towerUpgradeService,
            ITowerService towerService,
            IWaveManager waveManager,
            CreateTowerPanelView createTowerPanelView,
            TowerDatabase towerDatabase,
            CoinCounterView coinCounterView,
            WaveCounterView waveCounterView,
            CoinCounterModel coinCounterModel,
            IInstantiator instantiator
        )
        {
            _gameSession = gameSession;
            _coinCounterModel = coinCounterModel;
            _analyticsService =  analyticsService;
            _towerUpgradeService = towerUpgradeService;
            _towerService = towerService;
            _waveManager = waveManager;
            _createTowerPanelView = createTowerPanelView;
            _towerDatabase = towerDatabase;
            _coinCounterView = coinCounterView;
            _waveCounterView = waveCounterView;
            _instantiator = instantiator;
        }

        public void Dispose()
        {
            if (_towerPanelPresenter != null)
                _towerPanelPresenter.Dispose();
        }

        public void CreateCoinCounterPanel() => _instantiator.InstantiatePrefab(_coinCounterView);

        public void CreateWaveCounterPanel() => _instantiator.InstantiatePrefab(_waveCounterView);
        
        public CreateTowerPanelPresenter CreateTowerPanel()
        {
            CreateTowerPanelModel towerPanelModel = new CreateTowerPanelModel(_towerDatabase.GetBuildable());
            CreateTowerPanelView towerPanelView = _instantiator.InstantiatePrefabForComponent<CreateTowerPanelView>(_createTowerPanelView);
            _towerPanelPresenter = new CreateTowerPanelPresenter(
                _instantiator,
                _coinCounterModel, 
                _analyticsService, 
                _towerService, 
                _waveManager, 
                _towerUpgradeService,
                _gameSession,
                towerPanelView,
                towerPanelModel);

            _towerPanelPresenter.Initialize();
            _towerPanelPresenter.HidePanel();
            
            return _towerPanelPresenter;
        }
    }
}