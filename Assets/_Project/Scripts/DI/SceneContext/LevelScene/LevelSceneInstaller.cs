using Zenject;
using UnityEngine;
using _Project.Scripts.UI;
using _Project.Scripts.Towers;
using _Project.Scripts.Weapons;
using _Project.Scripts.Enemies;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.Services.Upgrade;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.UI.Modals.EndGameModal;
using _Project.Scripts.Database.EnemyPrefabDatabase;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.DI.SceneContext.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _towerOccupiedLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private RectTransform _hud;
        [SerializeField] private EndGameModal _endGameModal;
        [SerializeField] private CoinCounterPanel _coinCounterPanel;
        [SerializeField] private WaveCounterPanel _waveCounterPanel;
        [SerializeField] private CreateTowerPanel _createTowerPanel;
        [SerializeField] private CreateTowerItemButton _createTowerItemButton;

        private GameRepository _gameRepository;
        private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        private UpgradeService _upgradeService;
        
        [Inject]
        public void Construct(
            EnemyPrefabsDatabase enemyPrefabsDatabase, 
            GameRepository gameRepository,
            UpgradeService upgradeService
            )
        {
            _upgradeService = upgradeService;
            _enemyPrefabsDatabase = enemyPrefabsDatabase;
            _gameRepository = gameRepository;
        }

        public override void InstallBindings()
        {
            BindEnemySpawner();
            BindEnemyPools();
            BindFactories();
            BindLevel();
        }

        private void BindEnemySpawner()
        {
            float viewHeight = _camera.orthographicSize * 2;
            float viewWidth = viewHeight * _camera.aspect;
            Container.Bind<EnemySpawner>().AsSingle().WithArguments(viewHeight, viewWidth);
        } 

        private void BindLevel()
        {
            Container.Bind<RectTransform>().WithId(GameConstants.HUD_INJECT_ID).FromInstance(_hud);
            Container.BindInterfacesAndSelfTo<TowerPlacement>().AsSingle().WithArguments(_camera, _towerOccupiedLayerMask, _groundLayerMask);

            Container.Bind<CoinCounterModel>().AsSingle();
            Container.Bind<EndGameModal>().FromInstance(_endGameModal).AsSingle();
            Container.BindInterfacesAndSelfTo<WaveManager>().AsSingle();
            Container.Bind<TowerService>().AsSingle();
            Container.Bind<CastleInitializer>().AsSingle();
            Container.Bind<EndGameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelBootstrapper>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<TowerFactory>().AsSingle();
            Container.Bind<WeaponFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle().WithArguments(
                _createTowerPanel, 
                _createTowerItemButton, 
                _coinCounterPanel,
                _waveCounterPanel);
        }

        private void BindEnemyPools()
        {
            Container.BindMemoryPool<Enemy, EnemyPool>()
                .FromComponentInNewPrefab(_enemyPrefabsDatabase.Get(EnemyType.Ork))
                .UnderTransformGroup("Enemies");
        }
    }
}
