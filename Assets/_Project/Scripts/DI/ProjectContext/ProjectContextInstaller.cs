using Zenject;
using UnityEngine;
using _Project.Scripts.Services;
using _Project.Scripts.Repositories;
using _Project.Scripts.Database.EnemyDatabase;
using _Project.Scripts.Database.TowersDatabase;
using _Project.Scripts.Infrastructure.SceneLoader;
using _Project.Scripts.Infrastructure.CoroutineRunner;

namespace _Project.Scripts.DI.ProjectContext
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private EnemyPrefabsDatabase _enemyPrefabsDatabase;
        [SerializeField] private TowerPrefabsDatabase _towerPrefabsDatabase;
        public override void InstallBindings()
        {
            _enemyPrefabsDatabase.Init();
            _towerPrefabsDatabase.Init();

            Container.BindInstance(_towerPrefabsDatabase).AsSingle();
            Container.BindInstance(_enemyPrefabsDatabase).AsSingle();
            
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<CoroutineRunner>().FromComponentInNewPrefab(_coroutineRunner).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyConfigsRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowersRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
        }
    }
}