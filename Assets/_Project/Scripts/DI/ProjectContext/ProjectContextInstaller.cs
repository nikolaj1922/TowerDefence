using Zenject;
using UnityEngine;
using _Project.Scripts.Database;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Services.Analytics.Firebase;
using _Project.Scripts.Database.EnemyPrefabDatabase;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Database.TowersPrefabDatabase;
using _Project.Scripts.Database.WeaponPrefabDatabase;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingScene;
using _Project.Scripts.Infrastructure.ModalCreator;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.DI.ProjectContext
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private LoadingCurtainView _loadingCurtainView;
        
        [SerializeField] private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        [SerializeField] private TowerPrefabsDatabase _towerPrefabsDatabase;
        [SerializeField] private WeaponPrefabsDatabase _weaponPrefabsDatabase;
        [SerializeField] private ModalsPrefabDatabase _modalPrefabsDatabase;
        [SerializeField] private UpgradesDatabase _upgradesDatabase;
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle().NonLazy();
            BindDatabases(); 
            
            Container.Bind<ModalCreator>().AsSingle();
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<ISaveLoad>().To<SaveLoad>().AsSingle();
            Container.Bind<TowerUpgradeService>().AsSingle();

            BindAnalytics();
            BindConfigRepositories();
            BindLoadingCurtain();
        }

        private void BindAnalytics()
        {
            Container.Bind<IAnalyticsClient>().To<FirebaseAnalyticsClient>().AsSingle();
            Container.Bind<AnalyticsService>().AsSingle();
        }

        private void BindDatabases()
        {
            _modalPrefabsDatabase.Init();
            _enemyPrefabsDatabase.Init();
            _towerPrefabsDatabase.Init();
            _weaponPrefabsDatabase.Init();
            Container.Bind<TowerPrefabsDatabase>().FromInstance(_towerPrefabsDatabase).AsSingle();
            Container.Bind<ModalsPrefabDatabase>().FromInstance(_modalPrefabsDatabase).AsSingle();
            Container.Bind<EnemyPrefabsDatabase>().FromInstance(_enemyPrefabsDatabase).AsSingle();
            Container.Bind<WeaponPrefabsDatabase>().FromInstance(_weaponPrefabsDatabase).AsSingle();
            Container.Bind<UpgradesDatabase>().FromInstance(_upgradesDatabase).AsSingle();
        }

        private void BindConfigRepositories()
        {
            Container.Bind<TowerConfigsRepository>().AsSingle();
            Container.Bind<WeaponConfigsRepository>().AsSingle();
            Container.Bind<EnemyConfigsRepository>().AsSingle();
            Container.Bind<GameRepository>().AsSingle();
        }

        private void BindLoadingCurtain()
        {
            Container.Bind<LoadingCurtainView>()
                .FromComponentInNewPrefab(_loadingCurtainView)
                .AsSingle();
            Container.Bind<LoadingCurtainModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingCurtainController>().AsSingle();
            Container.Bind<LoadingPipelineFactory>().AsSingle();
        }
    }
}