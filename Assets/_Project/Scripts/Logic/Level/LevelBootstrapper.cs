using System;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.GameSession;
using _Project.Scripts.Towers;
using _Project.Scripts.UI.CoinCounter;
using Zenject;

namespace _Project.Scripts.Logic.Level
{
    public class LevelBootstrapper : IInitializable, IDisposable
    {
        private readonly IGameFlowService _gameFlowService;
        private readonly ILevelUIService _levelUiService;
        private readonly ICastleService _castleService;
        private readonly ITowerPlacement _towerPlacement;
        private readonly IAnalyticsService _analyticsService;
        private readonly IWaveManager _waveManager;
        private readonly IGameSessionService _gameSessionService;
        private readonly CoinCounterModel _coinCounter;

        public LevelBootstrapper(
            CoinCounterModel coinCounter,
            IAnalyticsService analyticsService,
            IGameFlowService gameFlowService,
            ILevelUIService levelUiService,
            ICastleService castleService,
            ITowerPlacement towerPlacement,
            IGameSessionService gameSessionService,
            IWaveManager waveManager
            )
        {
            _gameSessionService = gameSessionService;
            _coinCounter = coinCounter;
            _waveManager = waveManager;
            _analyticsService = analyticsService;
            _gameFlowService = gameFlowService;
            _levelUiService = levelUiService;
            _castleService = castleService;
            _towerPlacement = towerPlacement;
        }

        public void Initialize()
        {
            _gameSessionService.ResetTowerOnLevel();
            
            _towerPlacement.OnPlaceClicked += _levelUiService.ShowTowerPanel;

            _castleService.OnDamaged += OnCastleDamaged;
            _castleService.OnDestroyed += OnDefeat;
            
            _waveManager.OnCompleteLevel += OnVictory;
            _waveManager.OnCompleteWave += OnCompleteWave;

            _gameFlowService.StartLevel();
        }
        
        public void Dispose()
        {
            _towerPlacement.OnPlaceClicked -= _levelUiService.ShowTowerPanel;

            _castleService.OnDamaged -= OnCastleDamaged;
            _castleService.OnDestroyed -= OnDefeat;
            
            _waveManager.OnCompleteLevel -= OnVictory;
            _waveManager.OnCompleteWave -= OnCompleteWave;
        }

        private void OnDefeat() => _gameFlowService.OnDefeat(_gameSessionService.TowerBuiltOnLevel);
        
        private void OnVictory() => _gameFlowService.OnVictory();
        
        private void OnCompleteWave(int wave) =>
            _analyticsService.WaveCompleted(wave, _gameSessionService.TowerBuiltOnLevel, _coinCounter.Coins);

        private void OnCastleDamaged(float hp) =>
            _analyticsService.CastleDamaged(_waveManager.CurrentWave, hp);
    }
}