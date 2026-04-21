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
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.DI.ProjectContext
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private AssetReference _menuScene;
        [SerializeField] private AssetReference _levelScene;
        [SerializeField] private AssetReferenceGameObject _loadingCurtain;
        [SerializeField] private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        [SerializeField] private TowerPrefabsDatabase _towerPrefabsDatabase;
        [SerializeField] private WeaponPrefabsDatabase _weaponPrefabsDatabase;
        [SerializeField] private ModalsPrefabsDatabase _modalPrefabsDatabase;
        [SerializeField] private UpgradeConfigsDatabase _upgradeConfigsDatabase;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle().NonLazy();
            BindDatabases(); 
            
            Container.Bind<IModalCreatorService>().To<ModalCreatorService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<ISaveLoad>().To<SaveLoad>().AsSingle();
            Container.Bind<ITowerUpgradeService>().To<TowerUpgradeService>().AsSingle();

            BindSceneAssets();
            BindAnalytics();
            BindLoadingCurtain();
        }

        private void BindSceneAssets()
        {
            Container.Bind<AssetReference>().FromInstance(_menuScene).AsSingle().WithConcreteId(GameConstants.MENU_SCENE);
            Container.Bind<AssetReference>().FromInstance(_levelScene).AsSingle().WithConcreteId(GameConstants.LEVEL_SCENE);
        }

        private void BindAnalytics()
        {
            Container.Bind<IAnalyticsClient>().To<FirebaseAnalyticsClient>().AsSingle();
            Container.Bind<IAnalyticsService>().To<AnalyticsService>().AsSingle();
        }

        private void BindDatabases()
        {
            _modalPrefabsDatabase.Init();
            Container.Bind<TowerPrefabsDatabase>().FromInstance(_towerPrefabsDatabase).AsSingle();
            Container.Bind<ModalsPrefabsDatabase>().FromInstance(_modalPrefabsDatabase).AsSingle();
            Container.Bind<EnemyPrefabsDatabase>().FromInstance(_enemyPrefabsDatabase).AsSingle();
            Container.Bind<WeaponPrefabsDatabase>().FromInstance(_weaponPrefabsDatabase).AsSingle();
            Container.Bind<UpgradeConfigsDatabase>().FromInstance(_upgradeConfigsDatabase).AsSingle();
            
            Container.Bind<TowerConfigsDatabase>().AsSingle();
            Container.Bind<WeaponConfigsDatabase>().AsSingle();
            Container.Bind<EnemyConfigsDatabase>().AsSingle();
            Container.Bind<GameConfigDatabase>().AsSingle();
        }

        private void BindLoadingCurtain()
        {
            Container.Bind<ILoadingCurtainFactory>().To<LoadingCurtainFactory>().AsSingle().WithArguments(_loadingCurtain);
            Container.Bind<ILoadingPipelineFactory>().To<LoadingPipelineFactory>().AsSingle();
        }
    }
}