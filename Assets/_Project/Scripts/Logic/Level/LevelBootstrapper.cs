using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.UI;
using _Project.Scripts.Tower;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.UI.CreateTowerPanel;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private Castle _castle;
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
            EndGameService endGameService,
            AnalyticsService analyticsService
            )
        {
            _coinCounterModel = coinCounterModel;
            _castleInitializer = castleInitializer;
            _towerPlacement = towerPlacement;
            _gameRepository = gameRepository;
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _towerService = towerService;
            _endGameService = endGameService;
            _analyticsService = analyticsService;
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
            _castle = _castleInitializer.CreateCastle(_gameRepository.GameConfig.castlePosition);
            _castle.OnCastleDamaged += OnCastleDamaged;
            _castle.OnCastleDestroy += GameOver;
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
                Debug.Log("Not enough coins!");
                _analyticsService.BuildRejected("not_enough_coins", _waveManager.CurrentWave);
                return;
            }   
            
            _totalTowersBuilt++;
            _towerService.CreateAndPurchase(towerType, position, coinPrice);
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
