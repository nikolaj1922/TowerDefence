using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Tower;
using _Project.Scripts.Weapon;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Tower.Castle.States;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Logic.Game
{
    public class GameManager : IInitializable, IDisposable
    {
        [Inject] private DiContainer _container;

        private WaveManager _waveManager;
        private UIManager _uiManager;
        private CoinCounterModel _coinCounterModel;
        
        private TowerFactory _towerFactory;
        private WeaponFactory _weaponFactory;
        private GameRepository _gameRepository;
        private TowerConfigsRepository _towerConfigsRepository;

        private TowerPlacement _towerPlacement;
        private CreateTowerPanel _createTowerPanel;
        private ISaveLoad _saveLoad;
        
        [Inject]
        private void Construct(
            TowerFactory towerFactory,
            WeaponFactory weaponFactory,
            GameRepository gameRepository,
            TowerConfigsRepository towerConfigsRepository,
            UIManager uiManager,
            TowerPlacement towerPlacement,
            WaveManager waveManager,
            CoinCounterModel coinCounterModel,
            ISaveLoad saveLoad
            )
        {
            _coinCounterModel = coinCounterModel;
            _towerPlacement = towerPlacement;
            _towerConfigsRepository = towerConfigsRepository;
            _weaponFactory = weaponFactory;
            _towerFactory = towerFactory;
            _gameRepository = gameRepository;
            _uiManager = uiManager;
            _waveManager = waveManager;
            _saveLoad = saveLoad;
        }

        public void Initialize()
        { 
            _uiManager.CreateCoinCounterPanel();
            _createTowerPanel = _uiManager.CreateTowerPanel(onCreateTowerClick: CreateTower);
            _createTowerPanel.OnShowPanel += _towerPlacement.DisableTowerPlacement;
            _createTowerPanel.OnHidePanel += _towerPlacement.EnableTowerPlacement;
            _towerPlacement.OnPlaceClicked += _createTowerPanel.ShowPanel;
            
            CreateCastle();
            _waveManager.StartNextWave();
        }

        private void CreateCastle()
        {
            Castle castle = (Castle)CreateTower(TowerType.Castle, _gameRepository.GameConfig.castlePosition, 0);

            StateMachine stateMachine = new StateMachine(
                new IState[]
                {
                    new EntireState(),
                    new CollapseState(castle)
                },
                new ITransition[]
                {
                    new Transition<EntireState, CollapseState>(
                        () => castle.HealthModel.CurrentHealth / castle.HealthModel.MaxHealth < GameConstants.CASTLE_COLLAPSE_HEALTH_PERCENT)
                }
            );
            
            castle.SetStateMachine(stateMachine);
            
            castle.OnCastleDestroy += GameOver;
        }

        private void GameOver()
        {
            int wavesSurvived = _waveManager.WaveIndex + 1;
            int metaCoinsAdded =
                wavesSurvived * _gameRepository.GameConfig.coinsPerWave
                + _waveManager.TotalEnemyKilled * _gameRepository.GameConfig.coinsPerKill;
            
            _saveLoad.AddMetaCoins(metaCoinsAdded);
            _uiManager.CreateDefeatModal(metaCoinsAdded);
        }

        private Tower.Tower CreateTower(TowerType towerType, Vector3 position, int coinPrice)
        {
            Tower.Tower tower = _towerFactory.CreateTower(towerType, position);
            Weapon.Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerConfigsRepository.Get(towerType).weaponType, 
                    tower.WeaponPoint.transform.position, 
                    tower.WeaponPoint.transform);
            
            tower.SetWeapon(weapon);
            _createTowerPanel.HidePanel();
            _coinCounterModel.RemoveCoins(coinPrice);

            return tower;
        }

        public void Dispose()
        {
            _createTowerPanel.OnShowPanel -= _towerPlacement.DisableTowerPlacement;
            _createTowerPanel.OnHidePanel -= _towerPlacement.EnableTowerPlacement;
            _towerPlacement.OnPlaceClicked -= _createTowerPanel.ShowPanel;
        }
    }
}
