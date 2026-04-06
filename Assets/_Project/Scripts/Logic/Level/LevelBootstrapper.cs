using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.UI;
using _Project.Scripts.Tower;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.Upgrade;
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
        private UpgradeService _upgradeService;

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
        }

        public void Initialize()
        {
            CreateUI();
            CreateCastle();
            
            _towerPlacement.OnPlaceClicked += _createTowerPanel.ShowPanel;
            _waveManager.OnCompleteLevel += GameVictory;
            _waveManager.StartTimer(waveCount: 1);
        }
        
        public void Dispose()
        {
            _towerPlacement.OnPlaceClicked -= _createTowerPanel.ShowPanel;
            _castle.OnCastleDestroy -= GameOver;
            _waveManager.OnCompleteLevel -= GameVictory;
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
            _castle.OnCastleDestroy += GameOver;
        }
        
        private void GameOver() => _endGameService.GameOver();
        
        private void GameVictory() => _endGameService.GameVictory();
        
        private Tower.Tower CreateTower(
            TowerType towerType, 
            Vector3 position, 
            int coinPrice)
        {
            Tower.Tower tower = _towerService.CreateAndPurchase(
                towerType, 
                position, 
                coinPrice,
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.TOWER_DAMAGE_ID),
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.TOWER_ATTACK_SPEED_ID)
                );
            
            _createTowerPanel.HidePanel();
            return tower;
        }
    }
}
