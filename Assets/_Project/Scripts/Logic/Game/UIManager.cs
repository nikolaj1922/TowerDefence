using System;
using Zenject;
using UnityEngine;
using Object = UnityEngine.Object;
using _Project.Scripts.Tower;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.UI.DefeatModal;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.Logic.Game
{
    public class UIManager
    {
        [Inject] private DiContainer _container;
        private DefeatModal _defeatModal;
        private RectTransform _hud;
        private CoinCounterModel _coinCounterModel;
        private CreateTowerPanel _createTowerPanel;
        private CreateTowerItemButton _createTowerItemButton;
        private TowerConfigsRepository _towerConfigsRepository;
        private CoinCounterPanel _coinCounterPanel;
        
        private Vector3 _createTowerPosition;

        [Inject]
        public void Construct(
            [Inject(Id = "HUD")] RectTransform hud,
            CoinCounterModel coinCounterModel,
            DefeatModal defeatModal, 
            CreateTowerPanel createTowerPanel,
            CreateTowerItemButton createTowerItemButton,
            TowerConfigsRepository towerConfigsRepository,
            CoinCounterPanel coinCounterPanel
        )
        {
            _hud = hud;
            _coinCounterModel = coinCounterModel;
            _defeatModal = defeatModal;
            _createTowerPanel = createTowerPanel;
            _createTowerItemButton = createTowerItemButton;
            _towerConfigsRepository = towerConfigsRepository;
            _coinCounterPanel = coinCounterPanel;
        }

        public void CreateDefeatModal(int metaCoinsAdded)
        {
            DefeatModal defeatModal = _container.InstantiatePrefabForComponent<DefeatModal>(_defeatModal);
            defeatModal.Initialize();
            defeatModal.SetMetaCoinText(metaCoinsAdded);
        }

        public void CreateCoinCounterPanel() => _container.InstantiatePrefabForComponent<CoinCounterPanel>(_coinCounterPanel.gameObject, _hud);

        public CreateTowerPanel CreateTowerPanel(Func<TowerType, Vector3, int, Tower.Tower> onCreateTowerClick)
        {
            CreateTowerPanel panel = Object.Instantiate(_createTowerPanel, _hud);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -GameConstants.TOWER_PANEL_OFFSET);
            
            foreach (TowerConfig towerConfig in _towerConfigsRepository.GetBuildable())
                CreateTowerItemButton(towerConfig, rect, panel, onCreateTowerClick);
            
            return panel;
        }
        
        private void CreateTowerItemButton(
            TowerConfig config, 
            RectTransform parent, 
            CreateTowerPanel panel,
            Func<TowerType, Vector3, int, Tower.Tower> onClick)
        {
            CreateTowerItemButton towerButton = Object.Instantiate(_createTowerItemButton, parent);
            towerButton.Initialize(
                config.coinPrice, 
                config.icon,
                onClick: () => onClick(config.towerType, panel.CreateTowerPosition, config.coinPrice),
                _coinCounterModel
                );
        }
    }
}