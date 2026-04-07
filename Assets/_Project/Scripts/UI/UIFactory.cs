using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using Object = UnityEngine.Object;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Towers;

namespace _Project.Scripts.UI
{
    public class UIFactory
    {
        [Inject] private DiContainer _container;
        private RectTransform _hud;
        private EndGameModal.EndGameModal _endGameModal;
        private CoinCounterModel _coinCounterModel;
        private CoinCounterPanel _coinCounterPanel;
        private WaveCounterPanel _waveCounterPanel;
        private CreateTowerPanel.CreateTowerPanel _createTowerPanel;
        private CreateTowerItemButton _createTowerItemButton;
        private TowerConfigsRepository _towerConfigsRepository;
        private Vector3 _createTowerPosition;

        [Inject]
        public void Construct(
            [Inject(Id = GameConstants.HUD_INJECT_ID)] RectTransform hud,
            CoinCounterModel coinCounterModel,
            EndGameModal.EndGameModal endGameModal, 
            CreateTowerPanel.CreateTowerPanel createTowerPanel,
            CreateTowerItemButton createTowerItemButton,
            TowerConfigsRepository towerConfigsRepository,
            CoinCounterPanel coinCounterPanel,
            WaveCounterPanel waveCounterPanel
        )
        {
            _hud = hud;
            _coinCounterModel = coinCounterModel;
            _endGameModal = endGameModal;
            _createTowerPanel = createTowerPanel;
            _createTowerItemButton = createTowerItemButton;
            _towerConfigsRepository = towerConfigsRepository;
            _coinCounterPanel = coinCounterPanel;
            _waveCounterPanel = waveCounterPanel;
        }

        public void CreateEndGameModal(int metaCoinsAdded, string headerText)
        {
            EndGameModal.EndGameModal endGameModal = _container.InstantiatePrefabForComponent<EndGameModal.EndGameModal>(_endGameModal);
            endGameModal.SetMetaCoinText(metaCoinsAdded);
            endGameModal.SetHeaderText(headerText);
        }

        public void CreateCoinCounterPanel() => _container.InstantiatePrefab(_coinCounterPanel.gameObject, _hud);
        public void CreateWaveCounterPanel() => _container.InstantiatePrefab(_waveCounterPanel.gameObject, _hud);

        public CreateTowerPanel.CreateTowerPanel CreateTowerPanel(CreateTowerDelegate onCreateTowerClick)
        {
            CreateTowerPanel.CreateTowerPanel panel = Object.Instantiate(_createTowerPanel, _hud);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -GameConstants.PANEL_OFFSET);
            
            foreach (TowerConfig towerConfig in _towerConfigsRepository.GetBuildable())
                CreateTowerItemButton(towerConfig, rect, panel, onCreateTowerClick);
            
            return panel;
        }
        
        private void CreateTowerItemButton(
            TowerConfig config, 
            RectTransform parent, 
            CreateTowerPanel.CreateTowerPanel panel,
            CreateTowerDelegate onClick)
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
    
    public delegate Tower CreateTowerDelegate(
        TowerType towerType,
        Vector3 position,
        int coinPrice
    );
}