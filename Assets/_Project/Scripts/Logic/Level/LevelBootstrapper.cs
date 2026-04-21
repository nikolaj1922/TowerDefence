using System;
using _Project.Scripts.Database.Game;
using Zenject;
using _Project.Scripts.UI;
using _Project.Scripts.Towers;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.Modals.EndGameModal;
using _Project.Scripts.UI.TowerCreation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private CastleTower _castle;
        private UIFactory _uiFactory;
        private IWaveManager _waveManager;
        private GameConfigDatabase _gameConfigDatabase;
        private TowerPlacement _towerPlacement;
        private CreateTowerPanelView _createTowerPanelView;
        private ICastleInitializer _castleInitializer;
        private IAnalyticsService _analyticsService;
        private CoinCounterModel _coinCounterModel;
        private IModalCreatorService _modalCreatorService;
        private ISaveLoad _saveLoad;
        private ITowerUpgradeService _towerUpgradeService;

        private int _totalTowersBuilt;

        [Inject]
        private void Construct(
            ISaveLoad saveLoad,
            GameConfigDatabase gameConfigDatabase,
            UIFactory uiFactory,
            TowerPlacement towerPlacement,
            IWaveManager waveManager,
            CoinCounterModel coinCounterModel,
            ICastleInitializer castleInitializer,
            IAnalyticsService analyticsService,
            ITowerUpgradeService upgradeService,
            IModalCreatorService modalCreatorService,
            ITowerUpgradeService towerUpgradeService
            )
        {
            _saveLoad = saveLoad;
            _castleInitializer = castleInitializer;
            _towerPlacement = towerPlacement;
            _gameConfigDatabase = gameConfigDatabase;
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _towerUpgradeService = towerUpgradeService;
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
                _gameConfigDatabase.GameConfig.CastlePosition,
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_DAMAGE_ID),
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_ATTACK_SPEED_ID)
                );
            _castle.OnCastleDestroy += OnDefeat;
            _castle.OnCastleDestroy += _waveManager.StopWave;
            _castle.OnCastleDamaged += OnCastleDamaged;
        }
        
        private async UniTask CreateEndGameModal(string headerText)
        {
            GameObject modalObject = await _modalCreatorService.OpenModal(ModalType.EndGame);
            EndGameModalView endGameModalView = modalObject.GetComponent<EndGameModalView>();
            
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
            CreateEndGameModal("Defeat!").Forget();
        }

        private void OnVictory()
        {
            SaveMetaCoins();
            CreateEndGameModal("Victory!").Forget();
        }
        
        private void SaveMetaCoins() => _saveLoad.AddMetaCoins(_waveManager.GetRewardForWaves());
    }
}
