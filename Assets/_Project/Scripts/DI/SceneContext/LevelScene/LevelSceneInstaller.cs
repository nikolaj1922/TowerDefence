using Zenject;
using UnityEngine;
using _Project.Scripts.UI;
using _Project.Scripts.Enemy;
using _Project.Scripts.Tower;
using _Project.Scripts.Weapon;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Database.EnemyPrefabDatabase;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.UI.Modals.EndGameModal;

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

        private ISaveLoad _saveLoad;
        private GameRepository _gameRepository;
        private EnemyPrefabsDatabase _enemyPrefabsDatabase;

        private PlayerProgress Progress => _saveLoad.PlayerProgress;

        [Inject]
        public void Construct(
            EnemyPrefabsDatabase enemyPrefabsDatabase, 
            GameRepository gameRepository,
            ISaveLoad saveLoad
            )
        {
            _saveLoad = saveLoad;
            _enemyPrefabsDatabase = enemyPrefabsDatabase;
            _gameRepository = gameRepository;
        }

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            BindCastleHealthModel();
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
            Container.BindInterfacesAndSelfTo<TowerPlacement>().AsSingle().WithArguments(_towerOccupiedLayerMask, _groundLayerMask);

            Container.Bind<CoinCounterModel>().AsSingle();
            Container.Bind<EndGameModal>().FromInstance(_endGameModal).AsSingle();
            Container.BindInterfacesAndSelfTo<WaveManager>().AsSingle();
            Container.Bind<TowerService>().AsSingle();
            Container.Bind<CastleInitializer>().AsSingle();
            Container.Bind<EndGameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelBootstrapper>().AsSingle();
        }

        private void BindCastleHealthModel()
        {
            float finalCastleHp = Progress.upgrades.castleHpMultiplier * _gameRepository.GameConfig.castleHealth;
            HealthModel healthModel = new HealthModel(finalCastleHp);
            Container
                .Bind<HealthModel>()
                .WithId(GameConstants.CASTLE_HEALTH_MODEL_INJECT_ID)
                .FromInstance(healthModel)
                .AsSingle();
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
            Container.BindMemoryPool<Enemy.Enemy, EnemyPool>()
                .FromComponentInNewPrefab(_enemyPrefabsDatabase.Get(EnemyType.Ork))
                .UnderTransformGroup("Enemies");
        }
    }
}
