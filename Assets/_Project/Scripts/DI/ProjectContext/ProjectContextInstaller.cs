using Zenject;
using UnityEngine;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Database.Enemies;
using _Project.Scripts.Database.Game;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Services.Analytics.Firebase;
using _Project.Scripts.Database.Towers;
using _Project.Scripts.Database.Upgrades;
using _Project.Scripts.Database.Weapons;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.SceneLoader;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.DI.ProjectContext
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private AssetReference _menuScene;
        [SerializeField] private AssetReference _levelScene;
        [SerializeField] private AssetReferenceGameObject _loadingCurtain;

        [SerializeField] private GameDatabase _gameDatabase;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        [SerializeField] private TowerDatabase _towerDatabase;
        [SerializeField] private WeaponDatabase _weaponDatabase;
        [SerializeField] private ModalsDatabase _modalDatabase;
        [SerializeField] private UpgradeDatabase _upgradeDatabase;

        public override void InstallBindings()
        {
            BindCamera();
            BindDatabases();
            BindServices();
            BindSceneAssets();
            BindAnalytics();
            BindLoadingCurtain();
        }

        private void BindCamera() => Container.Bind<Camera>().FromInstance(_camera).AsSingle();

        private void BindServices()
        {
            Container.Bind<IModalCreatorService>().To<ModalCreatorService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<ISaveLoad>().To<SaveLoad>().AsSingle();
            Container.Bind<ITowerUpgradeService>().To<TowerUpgradeService>().AsSingle();
        }

        private void BindSceneAssets()
        {
            Container.Bind<AssetReference>().WithId(GameConstants.MENU_SCENE).FromInstance(_menuScene);
            Container.Bind<AssetReference>().WithId(GameConstants.LEVEL_SCENE).FromInstance(_levelScene);
        }

        private void BindAnalytics()
        {
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle().NonLazy();
            Container.Bind<IAnalyticsClient>().To<FirebaseAnalyticsClient>().AsSingle();
            Container.Bind<IAnalyticsService>().To<AnalyticsService>().AsSingle();
        }

        private void BindDatabases()
        {
            _modalDatabase.Init();
            Container.Bind<TowerDatabase>().FromInstance(_towerDatabase).AsSingle();
            Container.Bind<ModalsDatabase>().FromInstance(_modalDatabase).AsSingle();
            Container.Bind<EnemyDatabase>().FromInstance(_enemyDatabase).AsSingle();
            Container.Bind<WeaponDatabase>().FromInstance(_weaponDatabase).AsSingle();
            Container.Bind<UpgradeDatabase>().FromInstance(_upgradeDatabase).AsSingle();
            Container.Bind<GameDatabase>().FromInstance(_gameDatabase).AsSingle();
        }

        private void BindLoadingCurtain()
        {
            Container.Bind<AssetReferenceGameObject>()
                .WithId(GameConstants.LOADING_CURTAIN_ASSET_INJECT_ID)
                .FromInstance(_loadingCurtain)
                .AsSingle();
            Container.Bind<LoadingCurtainModel>().AsSingle();
            Container.Bind<LoadingCurtainPresenter>().AsSingle();
            Container.Bind<ILoadingPipelineFactory>().To<LoadingPipelineFactory>().AsSingle();
        }
    }
}