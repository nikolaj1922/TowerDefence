using System;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Towers;
using Zenject;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private readonly IGameFlowService _gameFlowService;
        private readonly ILevelUIService _levelUiService;
        private readonly ICastleService _castleService;
        private readonly ITowerPlacement _towerPlacement;
        private readonly ILevelAnalyticsService _levelAnalyticsService;
        private readonly IWaveManager _waveManager;

        private int _totalTowersBuilt;

        public LevelBootstrapper(
            ILevelAnalyticsService levelAnalyticsService,
            IGameFlowService gameFlowService,
            ILevelUIService levelUiService,
            ICastleService castleService,
            ITowerPlacement towerPlacement,
            IWaveManager waveManager
            )
        {
            _waveManager = waveManager;
            _levelAnalyticsService = levelAnalyticsService;
            _gameFlowService = gameFlowService;
            _levelUiService = levelUiService;
            _castleService = castleService;
            _towerPlacement = towerPlacement;
        }

        public void Initialize()
        {
            _levelUiService.OnCreateTower += OnTowerCreated;
            _towerPlacement.OnPlaceClicked += _levelUiService.ShowTowerPanel;

            _castleService.OnDamaged += _levelAnalyticsService.OnCastleDamaged;
            _castleService.OnDestroyed += OnDefeat;
            
            _waveManager.OnCompleteLevel += OnVictory;
            _waveManager.OnCompleteWave += OnCompleteWave;

            _gameFlowService.StartLevel();
        }
        
        public void Dispose()
        {
            _levelUiService.OnCreateTower -= OnTowerCreated;
            _towerPlacement.OnPlaceClicked -= _levelUiService.ShowTowerPanel;

            _castleService.OnDamaged -= _levelAnalyticsService.OnCastleDamaged;
            _castleService.OnDestroyed -= OnDefeat;
            
            _waveManager.OnCompleteLevel -= OnVictory;
            _waveManager.OnCompleteWave -= OnCompleteWave;
        }

        private void OnTowerCreated(int price)
        {
            _totalTowersBuilt++;
            _levelAnalyticsService.OnTowerBuilt(price, _totalTowersBuilt);
        }

        private void OnDefeat() => _gameFlowService.OnDefeat(_totalTowersBuilt);
        
        private void OnVictory() => _gameFlowService.OnVictory();
        
        private void OnCompleteWave(int wave) => _levelAnalyticsService.OnWaveCompleted(wave, _totalTowersBuilt);
    }
}