using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Tower;
using _Project.Scripts.Weapon;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Tower.Castle.States;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Logic.Game
{
    public class GameManager : IInitializable, IDisposable
    {
        [Inject] private DiContainer _container;

        private UIFactory _uiFactory;
        private WaveManager _waveManager;
        private CoinCounterModel _coinCounterModel;
        
        private TowerFactory _towerFactory;
        private WeaponFactory _weaponFactory;
        private GameRepository _gameRepository;
        private TowerConfigsRepository _towerConfigsRepository;

        private Castle _castle;
        private ISaveLoad _saveLoad;
        private TowerPlacement _towerPlacement;
        private CreateTowerPanel _createTowerPanel;

        [Inject]
        private void Construct(
            TowerFactory towerFactory,
            WeaponFactory weaponFactory,
            GameRepository gameRepository,
            TowerConfigsRepository towerConfigsRepository,
            UIFactory uiFactory,
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
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _saveLoad = saveLoad;
        }

        public void Initialize()
        { 
            _uiFactory.CreateCoinCounterPanel();
            _uiFactory.CreateWaveCounterPanel();
            
            _createTowerPanel = _uiFactory.CreateTowerPanel(onCreateTowerClick: CreateTower);
            
            _towerPlacement.OnPlaceClicked += _createTowerPanel.ShowPanel;
            
            _waveManager.OnCompleteLevel += GameVictory;
            _waveManager.StartTimer(waveCount: 1);
            
            CreateCastle();
        }
        
        public void Dispose()
        {
            _towerPlacement.OnPlaceClicked -= _createTowerPanel.ShowPanel;
            _castle.OnCastleDestroy -= GameOver;
            _waveManager.OnCompleteLevel -= GameVictory;
        }

        private void CreateCastle()
        {
            _castle = (Castle)CreateTower(
                TowerType.Castle, 
                _gameRepository.GameConfig.castlePosition,
                coinPrice: 0, 
                isInitial: true);

            StateMachine stateMachine = new StateMachine(
                new IState[]
                {
                    new EntireState(),
                    new CollapseState(_castle)
                },
                new ITransition[]
                {
                    new Transition<EntireState, CollapseState>(
                        () => _castle.HealthModel.CurrentHealth / _castle.HealthModel.MaxHealth < GameConstants.CASTLE_COLLAPSE_HEALTH_PERCENT)
                }
            );
            
            _castle.SetStateMachine(stateMachine);
            _castle.OnCastleDestroy += GameOver;
        }

        private void EndLevel(string headerText)
        {
            int metaAdded = _waveManager.GetRewardForWaves();
            _saveLoad.AddMetaCoins(metaAdded);
            _uiFactory.CreateEndGameModal(metaAdded, headerText);
        }

        private void GameOver() => EndLevel("Defeat!");
        
        private void GameVictory() => EndLevel("Victory!");
        
        private void PurchaseTower(Tower.Tower tower, int coinPrice)
        {
            _coinCounterModel.RemoveCoins(coinPrice);
            _createTowerPanel.HidePanel();
        }

        private Tower.Tower CreateTower(TowerType towerType, Vector3 position, int coinPrice, bool isInitial = false)
        {
            Tower.Tower tower = _towerFactory.CreateTower(towerType, position);
            Weapon.Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerConfigsRepository.Get(towerType).weaponType, 
                    tower.WeaponPoint.transform.position, 
                    tower.WeaponPoint.transform);
            
            tower.SetWeapon(weapon);

            if (isInitial)
                return tower;

            PurchaseTower(tower, coinPrice);

            return tower;
        }
    }
    
    public delegate Tower.Tower CreateTowerDelegate(
        TowerType towerType,
        Vector3 position,
        int coinPrice,
        bool isInitial
    );
}
