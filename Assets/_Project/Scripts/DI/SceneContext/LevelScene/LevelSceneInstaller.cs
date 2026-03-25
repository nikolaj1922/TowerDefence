using Zenject;
using UnityEngine;
using _Project.Scripts.Enemy;
using _Project.Scripts.Logic.Game;
using _Project.Scripts.Tower;
using _Project.Scripts.Weapon;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.UI.DefeatModal;
using _Project.Scripts.UI.CreateTowerPanel;
using _Project.Scripts.Database.EnemyDatabase;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.UI.CoinCounter;

namespace _Project.Scripts.DI.SceneContext.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _towerLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;

        [SerializeField] private RectTransform _hud;
        [SerializeField] private DefeatModal _defeatModal;
        [SerializeField] private CoinCounterPanel _coinCounterPanel;
        [SerializeField] private CreateTowerPanel _createTowerPanel;
        [SerializeField] private CreateTowerItemButton _createTowerItemButton;

        private GameRepository _gameRepository;
        private EnemyPrefabsDatabase _enemyPrefabsDatabase;

        [Inject]
        public void Construct(EnemyPrefabsDatabase enemyPrefabsDatabase, GameRepository gameRepository)
        {
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
            Container.Bind<RectTransform>().WithId("HUD").FromInstance(_hud);
            Container.BindInterfacesAndSelfTo<TowerPlacement>().AsSingle().WithArguments(_towerLayerMask, _groundLayerMask);

            Container.Bind<CoinCounterModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<WaveManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIManager>().AsSingle().WithArguments(
                _defeatModal, 
                _createTowerPanel, 
                _createTowerItemButton, 
                _coinCounterPanel);
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        }

        private void BindCastleHealthModel()
        {
            HealthModel healthModel = new HealthModel(_gameRepository.GameConfig.castleHealth);
            Container
                .Bind<HealthModel>()
                .WithId("CastleHealthModel")
                .FromInstance(healthModel)
                .AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<TowerFactory>().AsSingle();
            Container.Bind<WeaponFactory>().AsSingle();
        }

        private void BindEnemyPools()
        {
            Container.BindMemoryPool<Enemy.Enemy, EnemyPool>()
                .FromComponentInNewPrefab(_enemyPrefabsDatabase.Get(EnemyType.Ork))
                .UnderTransformGroup("Enemies");
        }
    }
}
