using Zenject;
using UnityEngine;
using _Project.Scripts.UI;
using _Project.Scripts.Towers;
using _Project.Scripts.Weapons;
using _Project.Scripts.Enemies;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Towers.Castle;
using _Project.Scripts.UI.WaveCounter;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.Database.EnemyPrefabDatabase;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.Services.EndGame;

namespace _Project.Scripts.DI.SceneContext.LevelScene
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _towerOccupiedLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private CoinCounterView _coinCounterView;
        [SerializeField] private WaveCounterView _waveCounterView;
        [SerializeField] private CreateTowerView _createTowerView;
        [SerializeField] private CreateTowerItemButton _createTowerItemButton;

        private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        
        [Inject]
        public void Construct(EnemyPrefabsDatabase enemyPrefabsDatabase) =>
            _enemyPrefabsDatabase = enemyPrefabsDatabase;

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
            Container.BindInterfacesAndSelfTo<TowerPlacement>().AsSingle().WithArguments(_camera, _towerOccupiedLayerMask, _groundLayerMask);

            Container.Bind<CoinCounterModel>().AsSingle();
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
                _createTowerView, 
                _createTowerItemButton, 
                _coinCounterView,
                _waveCounterView);
        }

        private void BindEnemyPools()
        {
            Container.BindMemoryPool<Enemy, EnemyPool>()
                .FromComponentInNewPrefab(_enemyPrefabsDatabase.Get(EnemyType.Ork))
                .UnderTransformGroup("Enemies");
        }
    }
}
