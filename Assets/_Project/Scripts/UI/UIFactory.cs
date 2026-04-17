using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.Upgrade;
using _Project.Scripts.Towers;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;

namespace _Project.Scripts.UI
{
    public class UIFactory
    {
        private Vector3 _createTowerPosition;
        
        private IInstantiator _instantiator;
        private IWaveManager _waveManager;
        private ITowerService _towerService;
        private IUpgradeService _upgradeService;
        private CoinCounterView _coinCounterView;
        private WaveCounterView _waveCounterView;
        private CoinCounterModel _coinCounterModel;
        private IAnalyticsService _analyticsService;
        private CreateTowerPanelView _createTowerPanelView;
        private CreateTowerButtonView _createTowerButtonView;
        private TowerConfigsRepository _towerConfigsRepository;

        [Inject]
        public void Construct(
            IAnalyticsService analyticsService,
            IUpgradeService upgradeService,
            ITowerService towerService,
            IWaveManager waveManager,
            CreateTowerPanelView createTowerPanelView,
            CreateTowerButtonView createTowerButtonView,
            TowerConfigsRepository towerConfigsRepository,
            CoinCounterView coinCounterView,
            WaveCounterView waveCounterView,
            CoinCounterModel coinCounterModel,
            IInstantiator instantiator
        )
        {
            _coinCounterModel = coinCounterModel;
            _analyticsService =  analyticsService;
            _upgradeService = upgradeService;
            _towerService = towerService;
            _waveManager = waveManager;
            _createTowerPanelView = createTowerPanelView;
            _createTowerButtonView = createTowerButtonView;
            _towerConfigsRepository = towerConfigsRepository;
            _coinCounterView = coinCounterView;
            _waveCounterView = waveCounterView;
            _instantiator = instantiator;
        }

        public void CreateCoinCounterPanel() => _instantiator.InstantiatePrefab(_coinCounterView);

        public void CreateWaveCounterPanel() => _instantiator.InstantiatePrefab(_waveCounterView.gameObject);
        
        public CreateTowerPanelView CreateTowerPanel(Action<int> onCreateTower)
        {
            CreateTowerPanelView towerPanelView = _instantiator.InstantiatePrefabForComponent<CreateTowerPanelView>(_createTowerPanelView);
            towerPanelView.Initialize(onCreateTower);
            towerPanelView.HidePanel();
            
            foreach (TowerConfig towerConfig in _towerConfigsRepository.GetBuildable())
            {
                CreateTowerButtonView towerButtonView =
                    _instantiator.InstantiatePrefabForComponent<CreateTowerButtonView>(
                        _createTowerButtonView, 
                        towerPanelView.PanelRectTransform
                        );
                towerButtonView.Initialize(towerConfig.Icon, towerConfig.CoinPrice);
                
                CreateTowerButtonPresenter createTowerButtonPresenter = new CreateTowerButtonPresenter(
                    _waveManager,
                    _towerService,
                    _upgradeService,
                    _analyticsService,
                    _coinCounterModel,
                    towerPanelView.GetCreateTowerPosition,
                    towerButtonView);
            
                createTowerButtonPresenter.Initialize(towerConfig.TowerType, towerConfig.CoinPrice);
                towerPanelView.RegisterButton(towerButtonView, createTowerButtonPresenter);
            }
            
            return towerPanelView;
        }
    }
}