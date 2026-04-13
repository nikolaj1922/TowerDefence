using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using Object = UnityEngine.Object;
using _Project.Scripts.Towers;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.UI
{
    public class UIFactory
    {
        private IInstantiator _instantiator;
        private RectTransform _hud;
        private CoinCounterModel _coinCounterModel;
        private CoinCounterPanel _coinCounterPanel;
        private WaveCounterPanel _waveCounterPanel;
        private CreateTowerPanel _createTowerPanel;
        private CreateTowerItemButton _createTowerItemButton;
        private TowerConfigsRepository _towerConfigsRepository;
        private Vector3 _createTowerPosition;

        [Inject]
        public void Construct(
            [Inject(Id = GameConstants.HUD_INJECT_ID)] RectTransform hud,
            CoinCounterModel coinCounterModel,
            CreateTowerPanel createTowerPanel,
            CreateTowerItemButton createTowerItemButton,
            TowerConfigsRepository towerConfigsRepository,
            CoinCounterPanel coinCounterPanel,
            WaveCounterPanel waveCounterPanel,
            IInstantiator instantiator
        )
        {
            _hud = hud;
            _coinCounterModel = coinCounterModel;
            _createTowerPanel = createTowerPanel;
            _createTowerItemButton = createTowerItemButton;
            _towerConfigsRepository = towerConfigsRepository;
            _coinCounterPanel = coinCounterPanel;
            _waveCounterPanel = waveCounterPanel;
            _instantiator = instantiator;
        }

        public void CreateCoinCounterPanel() => _instantiator.InstantiatePrefab(_coinCounterPanel.gameObject, _hud);
        public void CreateWaveCounterPanel() => _instantiator.InstantiatePrefab(_waveCounterPanel.gameObject, _hud);

        public CreateTowerPanel CreateTowerPanel(CreateTowerDelegate onCreateTowerClick)
        {
            CreateTowerPanel towerPanel = _instantiator.InstantiatePrefabForComponent<CreateTowerPanel>(_createTowerPanel, _hud);
            
            RectTransform rect = towerPanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -GameConstants.PANEL_OFFSET);
            
            foreach (TowerConfig towerConfig in _towerConfigsRepository.GetBuildable())
                CreateTowerItemButton(towerConfig, rect, towerPanel, onCreateTowerClick);
            
            return towerPanel;
        }
        
        private void CreateTowerItemButton(
            TowerConfig config, 
            RectTransform parent, 
            CreateTowerPanel panel,
            CreateTowerDelegate onClick)
        {
            CreateTowerItemButton towerButton = Object.Instantiate(_createTowerItemButton, parent);
            
            towerButton.Initialize(
                config.CoinPrice, 
                config.Icon,
                onClick: () => onClick(config.TowerType, panel.CreateTowerPosition, config.CoinPrice),
                _coinCounterModel
                );
        }
    }
    
    public delegate void CreateTowerDelegate(
        TowerType towerType,
        Vector3 position,
        int coinPrice
    );
}