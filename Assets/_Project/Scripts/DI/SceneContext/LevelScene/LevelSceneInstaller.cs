using _Project.Scripts.Database.Enemies;
using Zenject;
using UnityEngine;
using _Project.Scripts.Towers;
using _Project.Scripts.Weapons;
using _Project.Scripts.Enemies;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.Enemies.Behaviour;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Logic.Level.Services;
using _Project.Scripts.UI;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;
using _Project.Scripts.UI.WaveCounter;

namespace _Project.Scripts.DI.SceneContext.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private LayerMask _towerOccupiedLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private CoinCounterView _coinCounterView;
        [SerializeField] private WaveCounterView _waveCounterView;
        [SerializeField] private CreateTowerPanelView _createTowerPanelView;
        [SerializeField] private CreateTowerButtonView _createTowerButtonView;
        
        private EnemyDatabase _enemyDatabase;
        private Camera _camera;

        [Inject]
        public void Construct(EnemyDatabase enemyDatabase, Camera gameCamera)
        {
            _enemyDatabase = enemyDatabase;
            _camera = gameCamera;
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
            float viewWidth = viewHeight * GameConstants.CAMERA_ASPECT_RATIO;
            
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().AsSingle().WithArguments(viewHeight, viewWidth);
        } 

        private void BindLevel()
        {
            Container
                .BindInterfacesAndSelfTo<TowerPlacement>()
                .AsSingle()
                .WithArguments(_camera, _towerOccupiedLayerMask, _groundLayerMask);

            Container.Bind<CoinCounterModel>().AsSingle();
            Container.Bind<IWaveManager>().To<WaveManager>().AsSingle();
            Container.Bind<ITowerService>().To<TowerService>().AsSingle();
            Container.Bind<ICastleInitializer>().To<CastleInitializer>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle().WithArguments(
                _createTowerPanelView, 
                _createTowerButtonView, 
                _coinCounterView,
                _waveCounterView);
            
            Container.BindInterfacesAndSelfTo<CastleService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelAnalyticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelUIService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameFlowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelBootstrapper>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<ITowerFactory>().To<TowerFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();
        }

        private void BindEnemyPools()
        {
            Container.BindMemoryPool<Enemy, EnemyPool>()
                .FromComponentInNewPrefab(_enemyDatabase.GetPrefab(EnemyType.Ork))
                .UnderTransformGroup("Enemies");
        }
    }
}
