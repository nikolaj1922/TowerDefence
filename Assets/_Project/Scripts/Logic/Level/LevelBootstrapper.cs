using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.UI;
using _Project.Scripts.Towers;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.Upgrade;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.UI.TowerCreation;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private CastleTower _castle;
        private UIFactory _uiFactory;
        private WaveManager _waveManager;
        private TowerService _towerService;
        private GameRepository _gameRepository;
        private TowerPlacement _towerPlacement;
        private EndGameService _endGameService;
        private CreateTowerPanel _createTowerPanel;
        private CastleInitializer _castleInitializer;
        private AnalyticsService _analyticsService;
        private CoinCounterModel _coinCounterModel;
        private UpgradeService _upgradeService;

        private int _totalTowersBuilt;

        [Inject]
        private void Construct(
            GameRepository gameRepository,
            UIFactory uiFactory,
            TowerPlacement towerPlacement,
            WaveManager waveManager,
            CoinCounterModel coinCounterModel,
            TowerService towerService,
            CastleInitializer castleInitializer,
            AnalyticsService analyticsService,
            EndGameService endGameService,
            UpgradeService upgradeService
            )
        {
            _castleInitializer = castleInitializer;
            _towerPlacement = towerPlacement;
            _gameRepository = gameRepository;
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _towerService = towerService;
            _endGameService = endGameService;
            _upgradeService = upgradeService;
            _analyticsService = analyticsService;
            _coinCounterModel = coinCounterModel;
        }

        public void Initialize()
        {
            CreateUI();
            CreateCastle();
            
            _towerPlacement.OnPlaceClicked += _createTowerPanel.ShowPanel;
            _waveManager.OnCompleteLevel += GameVictory;
            _waveManager.OnCompleteWave += OnCompleteWave;
            _waveManager.StartTimer(waveCount: 1);
        }
        
        public void Dispose()
        {
            _towerPlacement.OnPlaceClicked -= _createTowerPanel.ShowPanel;
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
            _createTowerPanel = _uiFactory.CreateTowerPanel(onCreateTowerClick: CreateTower);
        }

        private void CreateCastle()
        {
            _castle = _castleInitializer.CreateCastle(
                _gameRepository.GameConfig.CastlePosition,
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.CASTLE_DAMAGE_ID),
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.CASTLE_ATTACK_SPEED_ID)
                );
            _castle.OnCastleDestroy += GameOver;
            _castle.OnCastleDestroy += _waveManager.StopWave;
            _castle.OnCastleDamaged += OnCastleDamaged;
        }
        
        private void GameOver() => _endGameService.GameOver(_totalTowersBuilt);
        
        private void GameVictory() => _endGameService.GameVictory();
        
        private void CreateTower(
            TowerType towerType, 
            Vector3 position, 
            int coinPrice)
        {
            if (_coinCounterModel.Coins < coinPrice)
            {
                _analyticsService.BuildRejected("not_enough_coins", _waveManager.CurrentWave);
                return;
            }   
            
            _totalTowersBuilt++;
            _towerService.CreateAndPurchase(
                towerType, 
                position, 
                coinPrice,
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.TOWER_DAMAGE_ID),
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.TOWER_ATTACK_SPEED_ID)
            );
            _createTowerPanel.HidePanel();
            
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
