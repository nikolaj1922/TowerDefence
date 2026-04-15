using System;
using Zenject;
using _Project.Scripts.UI;
using _Project.Scripts.Towers;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.Services.EndGame;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private CastleTower _castle;
        private UIFactory _uiFactory;
        private WaveManager _waveManager;
        private GameRepository _gameRepository;
        private TowerPlacement _towerPlacement;
        private EndGameService _endGameService;
        private CreateTowerView _createTowerView;
        private CastleInitializer _castleInitializer;
        private AnalyticsService _analyticsService;
        private CoinCounterModel _coinCounterModel;
        private TowerUpgradeService _towerUpgradeService;

        private int _totalTowersBuilt;

        [Inject]
        private void Construct(
            GameRepository gameRepository,
            UIFactory uiFactory,
            TowerPlacement towerPlacement,
            WaveManager waveManager,
            CoinCounterModel coinCounterModel,
            CastleInitializer castleInitializer,
            AnalyticsService analyticsService,
            EndGameService endGameService,
            TowerUpgradeService towerUpgradeService
            )
        {
            _castleInitializer = castleInitializer;
            _towerPlacement = towerPlacement;
            _gameRepository = gameRepository;
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _endGameService = endGameService;
            _towerUpgradeService = towerUpgradeService;
            _analyticsService = analyticsService;
            _coinCounterModel = coinCounterModel;
        }

        public void Initialize()
        {
            CreateUI();
            CreateCastle();
            
            _towerPlacement.OnPlaceClicked += _createTowerView.ShowPanel;
            _waveManager.OnCompleteLevel += GameVictory;
            _waveManager.OnCompleteWave += OnCompleteWave;
            _waveManager.StartTimer(waveCount: 1);
        }
        
        public void Dispose()
        {
            _towerPlacement.OnPlaceClicked -= _createTowerView.ShowPanel;
            _castle.OnCastleDestroy -= GameOver;
            _castle.OnCastleDestroy -= _waveManager.StopWave;
            _castle.OnCastleDamaged -= OnCastleDamaged;
            _waveManager.OnCompleteLevel -= GameVictory;
            _waveManager.OnCompleteWave -= OnCompleteWave;
        }

        private void CreateUI()
        {
            _uiFactory.CreateCoinCounterPanel();
            _uiFactory.CreateWaveCounterPanel();
            _createTowerView = _uiFactory.CreateTowerPanel(onCreateTower: OnCreateTower);
        }

        private void CreateCastle()
        {
            _castle = _castleInitializer.CreateCastle(
                _gameRepository.GameConfig.CastlePosition,
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_DAMAGE_ID),
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_ATTACK_SPEED_ID)
                );
            _castle.OnCastleDestroy += GameOver;
            _castle.OnCastleDestroy += _waveManager.StopWave;
            _castle.OnCastleDamaged += OnCastleDamaged;
        }
        
        private void GameOver() => _endGameService.GameOver(_totalTowersBuilt);
        
        private void GameVictory() => _endGameService.GameVictory();
        
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
    }
}
