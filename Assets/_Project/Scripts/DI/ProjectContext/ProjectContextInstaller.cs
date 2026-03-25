using Zenject;
using UnityEngine;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Database.EnemyDatabase;
using _Project.Scripts.Database.TowersDatabase;
using _Project.Scripts.Database.WeaponDatabase;
using _Project.Scripts.Infrastructure.SceneLoader;
using _Project.Scripts.Infrastructure.CoroutineRunner;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.DI.ProjectContext
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        [SerializeField] private TowerPrefabsDatabase _towerPrefabsDatabase;
        [SerializeField] private WeaponPrefabsDatabase _weaponPrefabsDatabase;
        
        public override void InstallBindings()
        {
            BindPrefabDatabases(); 
            
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.Bind<CoroutineRunner>().FromComponentInNewPrefab(_coroutineRunner).AsSingle().NonLazy();
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<ISaveLoad>().To<SaveLoad>().AsSingle();

            BindConfigRepositories();
        }

        private void BindPrefabDatabases()
        {
            _enemyPrefabsDatabase.Init();
            _towerPrefabsDatabase.Init();
            _weaponPrefabsDatabase.Init();
            Container.Bind<TowerPrefabsDatabase>().FromInstance(_towerPrefabsDatabase).AsSingle();
            Container.Bind<EnemyPrefabsDatabase>().FromInstance(_enemyPrefabsDatabase).AsSingle();
            Container.Bind<WeaponPrefabsDatabase>().FromInstance(_weaponPrefabsDatabase).AsSingle();
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