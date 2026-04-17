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
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SceneLoader;
using _Project.Scripts.Services.Upgrade;

namespace _Project.Scripts.DI.ProjectContext
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        [SerializeField] private TowerPrefabsDatabase _towerPrefabsDatabase;
        [SerializeField] private WeaponPrefabsDatabase _weaponPrefabsDatabase;
        [SerializeField] private ModalsPrefabDatabase _modalPrefabsDatabase;
        [SerializeField] private UpgradesDatabase _upgradesDatabase;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle().NonLazy();
            BindDatabases(); 
            
            Container.Bind<IModalCreatorService>().To<ModalCreatorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<ISaveLoad>().To<SaveLoad>().AsSingle();
            Container.Bind<IUpgradeService>().To<UpgradeService>().AsSingle();
            
            Container.Bind<IAnalyticsClient>().To<FirebaseAnalyticsClient>().AsSingle();
            Container.Bind<IAnalyticsService>().To<AnalyticsService>().AsSingle();

            BindConfigRepositories();
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
    }
}