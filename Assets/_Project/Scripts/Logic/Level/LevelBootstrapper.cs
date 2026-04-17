using System;
using Zenject;
using _Project.Scripts.UI;
using _Project.Scripts.Towers;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Upgrade;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.Modals.EndGameModal;
using _Project.Scripts.UI.TowerCreation;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private CastleTower _castle;
        private UIFactory _uiFactory;
        private IWaveManager _waveManager;
        private GameRepository _gameRepository;
        private TowerPlacement _towerPlacement;
        private CreateTowerPanelView _createTowerPanelView;
        private ICastleInitializer _castleInitializer;
        private IAnalyticsService _analyticsService;
        private CoinCounterModel _coinCounterModel;
        private IUpgradeService _upgradeService;
        private IModalCreatorService _modalCreatorService;
        private ISaveLoad _saveLoad;

        private int _totalTowersBuilt;

        [Inject]
        private void Construct(
            ISaveLoad saveLoad,
            GameRepository gameRepository,
            UIFactory uiFactory,
            TowerPlacement towerPlacement,
            IWaveManager waveManager,
            CoinCounterModel coinCounterModel,
            ICastleInitializer castleInitializer,
            IAnalyticsService analyticsService,
            IUpgradeService upgradeService,
            IModalCreatorService modalCreatorService
            )
        {
            _saveLoad = saveLoad;
            _castleInitializer = castleInitializer;
            _towerPlacement = towerPlacement;
            _gameRepository = gameRepository;
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _upgradeService = upgradeService;
            _analyticsService = analyticsService;
            _coinCounterModel = coinCounterModel;
            _modalCreatorService = modalCreatorService;
        }

        public void Initialize()
        {
            CreateUI();
            CreateCastle();
            
            _towerPlacement.OnPlaceClicked += _createTowerPanelView.ShowPanel;
            _waveManager.OnCompleteLevel += OnVictory;
            _waveManager.OnCompleteWave += OnCompleteWave;
            _waveManager.StartTimer(waveCount: 1);
        }
        
        public void Dispose()
        {
            _towerPlacement.OnPlaceClicked -= _createTowerPanelView.ShowPanel;
            _castle.OnCastleDestroy -= OnDefeat;
            _castle.OnCastleDestroy -= _waveManager.StopWave;
            _castle.OnCastleDamaged -= OnCastleDamaged;
            _waveManager.OnCompleteLevel -= OnVictory;
            _waveManager.OnCompleteWave -= OnCompleteWave;
        }

        private void CreateUI()
        {
            _uiFactory.CreateCoinCounterPanel();
            _uiFactory.CreateWaveCounterPanel();
            _createTowerPanelView = _uiFactory.CreateTowerPanel(onCreateTower: OnCreateTower);
        }

        private void CreateCastle()
        {
            _castle = _castleInitializer.CreateCastle(
                _gameRepository.GameConfig.CastlePosition,
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.CASTLE_DAMAGE_ID),
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.CASTLE_ATTACK_SPEED_ID)
                );
            _castle.OnCastleDestroy += OnDefeat;
            _castle.OnCastleDestroy += _waveManager.StopWave;
            _castle.OnCastleDamaged += OnCastleDamaged;
        }
        
        private void CreateEndGameModal(string headerText)
        {
            EndGameModalView endGameModalView =
                _modalCreatorService.OpenModal(ModalType.EndGame)
                    .GetComponent<EndGameModalView>();
            endGameModalView.SetCurrentWave(_waveManager.CurrentWave);
            endGameModalView.Draw(headerText, _waveManager.GetRewardForWaves());
        }
        
        private void OnCreateTower(int coinPrice)
        {
            _totalTowersBuilt++;
            
            _analyticsService.TowerBuilt(
                _waveManager.CurrentWave,
                coinPrice,
                _coinCounterModel.Coins,
                _totalTowersBuilt);
        }
        
        private void OnCompleteWave(int wave) => _analyticsService.WaveCompleted(wave, _totalTowersBuilt, _coinCounterModel.Coins);
        
        private void OnCastleDamaged(float currentHealth) => _analyticsService.CastleDamaged(_waveManager.CurrentWave, currentHealth);
        
        private void OnDefeat()
        {
            _analyticsService.GameOver(
                _waveManager.CurrentWave, 
                _waveManager.TotalEnemyKilled, 
                _totalTowersBuilt, 
                _waveManager.GetRewardForWaves());
            SaveMetaCoins();
            CreateEndGameModal("Defeat!");
        }

        private void OnVictory()
        {
            SaveMetaCoins();
            CreateEndGameModal("Victory!");
        }
        
        private void SaveMetaCoins() => _saveLoad.AddMetaCoins(_waveManager.GetRewardForWaves());
    }
}
