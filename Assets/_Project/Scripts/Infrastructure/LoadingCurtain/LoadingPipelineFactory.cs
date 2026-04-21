using System.Collections.Generic;
using _Project.Scripts.Database.Enemies;
using _Project.Scripts.Database.Game;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Database.Towers;
using _Project.Scripts.Database.Upgrades;
using _Project.Scripts.Database.Weapons;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.SceneLoader;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingPipelineFactory : ILoadingPipelineFactory
    {
        private readonly ISaveLoad _saveLoad;
        private readonly ISceneLoader _sceneSceneLoader;
        private readonly IModalCreatorService _modalCreatorService;

        private readonly UpgradeConfigsDatabase _upgradeConfigDatabase;
        private readonly TowerPrefabsDatabase _towerPrefabsDatabase;
        private readonly EnemyPrefabsDatabase _enemyPrefabsDatabase;
        private readonly WeaponPrefabsDatabase _weaponPrefabsDatabase;
        
        private readonly GameConfigDatabase _gameConfigDatabase;
        private readonly EnemyConfigsDatabase _enemyConfigsDatabase;
        private readonly TowerConfigsDatabase _towerConfigsDatabase;
        private readonly WeaponConfigsDatabase _weaponConfigsDatabase;
        
        private readonly AssetReference _menuSceneReference;
        private readonly AssetReference _levelSceneReference;

        public LoadingPipelineFactory(
            IModalCreatorService modalCreatorService,
            ISaveLoad saveLoad,
            ISceneLoader sceneSceneLoader,
            GameConfigDatabase gameConfigDatabase,
            TowerConfigsDatabase towerConfigsDatabase,
            EnemyConfigsDatabase enemyConfigsDatabase,
            WeaponConfigsDatabase weaponConfigsDatabase,
            UpgradeConfigsDatabase upgradeConfigDatabase,
            TowerPrefabsDatabase towerPrefabsDatabase,
            WeaponPrefabsDatabase weaponPrefabsDatabase,
            EnemyPrefabsDatabase enemyPrefabsDatabase,
            [Inject(Id = GameConstants.MENU_SCENE)]
            AssetReference menuSceneReference,
            [Inject(Id = GameConstants.LEVEL_SCENE)]
            AssetReference levelSceneReference
        )
        {
            _modalCreatorService = modalCreatorService;
            _saveLoad = saveLoad;
            _sceneSceneLoader = sceneSceneLoader;

            _towerConfigsDatabase = towerConfigsDatabase;
            _gameConfigDatabase = gameConfigDatabase;
            _enemyConfigsDatabase = enemyConfigsDatabase;
            _weaponConfigsDatabase = weaponConfigsDatabase;

            _upgradeConfigDatabase = upgradeConfigDatabase;
            _towerPrefabsDatabase = towerPrefabsDatabase;
            _weaponPrefabsDatabase = weaponPrefabsDatabase;
            _enemyPrefabsDatabase = enemyPrefabsDatabase;

            _menuSceneReference = menuSceneReference;
            _levelSceneReference = levelSceneReference;
        }

        public Queue<ILoadingOperation> StartGamePipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new LoadPlayerProgressOperation(_saveLoad),
                new LoadDatabaseOperation(new List<UniTask> { _upgradeConfigDatabase.Load() }, "Load upgrades"),
                new LoadSceneOperation(
                    _sceneSceneLoader, 
                    _menuSceneReference.RuntimeKey.ToString(),
                    () => _modalCreatorService.OpenModal(ModalType.Menu).Forget())
            });
        } 
        
        public Queue<ILoadingOperation> LevelPipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new LoadDatabaseOperation(new List<UniTask>
                {
                    _enemyConfigsDatabase.Load(),
                    _weaponConfigsDatabase.Load(),
                    _towerConfigsDatabase.Load(),
                    _gameConfigDatabase.Load(),
                }, "Load configs"),
                new LoadDatabaseOperation(new List<UniTask>
                {
                    _towerPrefabsDatabase.Load(),
                    _weaponPrefabsDatabase.Load(),
                    _enemyPrefabsDatabase.Load()
                }, "Load prefabs"),
                new LoadSceneOperation(
                    _sceneSceneLoader, 
                    _levelSceneReference.RuntimeKey.ToString())
            });
        }
        
        public Queue<ILoadingOperation> RestartLevelPipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new LoadSceneOperation(
                    _sceneSceneLoader, 
                    _levelSceneReference.RuntimeKey.ToString())
            });
        }
    }
}