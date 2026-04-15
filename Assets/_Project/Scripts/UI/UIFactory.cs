using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.UI
{
    public class UIFactory
    {
        private IInstantiator _instantiator;
        private CoinCounterView _coinCounterView;
        private WaveCounterView _waveCounterView;
        private CreateTowerView _createTowerView;
        private CreateTowerItemButton _createTowerItemButton;
        private TowerConfigsRepository _towerConfigsRepository;
        private Vector3 _createTowerPosition;

        [Inject]
        public void Construct(
            CreateTowerView createTowerView,
            CreateTowerItemButton createTowerItemButton,
            TowerConfigsRepository towerConfigsRepository,
            CoinCounterView coinCounterView,
            WaveCounterView waveCounterView,
            IInstantiator instantiator
        )
        {
            _createTowerView = createTowerView;
            _createTowerItemButton = createTowerItemButton;
            _towerConfigsRepository = towerConfigsRepository;
            _coinCounterView = coinCounterView;
            _waveCounterView = waveCounterView;
            _instantiator = instantiator;
        }

        public void CreateCoinCounterPanel() => _instantiator.InstantiatePrefab(_coinCounterView.gameObject);
        public void CreateWaveCounterPanel() => _instantiator.InstantiatePrefab(_waveCounterView.gameObject);

        public CreateTowerView CreateTowerPanel(Action<int> onCreateTower)
        {
            CreateTowerView towerView = _instantiator.InstantiatePrefabForComponent<CreateTowerView>(_createTowerView);
            towerView.Initialize(onCreateTower);
            towerView.HidePanel();
            
            foreach (TowerConfig towerConfig in _towerConfigsRepository.GetBuildable())
            {
                CreateTowerItemButton towerButton = CreateTowerItemButton(towerConfig, towerView.PanelRectTransform, towerView);
                towerView.RegisterButton(towerButton);
            }
            
            return towerView;
        }
        
        private CreateTowerItemButton CreateTowerItemButton(
            TowerConfig config, 
            RectTransform parent, 
            CreateTowerView view
            )
        {
            CreateTowerItemButton towerButton =
                _instantiator.InstantiatePrefabForComponent<CreateTowerItemButton>(_createTowerItemButton, parent);
            
            towerButton.Initialize(
                config.CoinPrice, 
                config.Icon,
                config.TowerType,
                view
                );

            return towerButton;
        }
    }
}