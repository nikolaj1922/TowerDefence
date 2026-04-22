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

namespace _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory
{
    public class LoadingPipelineFactory : ILoadingPipelineFactory
    {
        private readonly ISaveLoad _saveLoad;
        private readonly ISceneLoader _sceneSceneLoader;
        private readonly IModalCreatorService _modalCreatorService;
        
        private readonly EnemyDatabase _enemyDatabase;
        private readonly TowerDatabase _towerDatabase;
        private readonly WeaponDatabase _weaponDatabase;
        private readonly UpgradeDatabase _upgradeDatabase;
        private readonly GameDatabase _gameDatabase;
        
        private readonly AssetReference _menuSceneReference;
        private readonly AssetReference _levelSceneReference;

        public LoadingPipelineFactory(
            IModalCreatorService modalCreatorService,
            ISaveLoad saveLoad,
            ISceneLoader sceneSceneLoader,
            
            GameDatabase gameDatabase,
            EnemyDatabase enemyDatabase,
            UpgradeDatabase upgradeDatabase,
            TowerDatabase towerDatabase,
            WeaponDatabase weaponDatabase,
            
            [Inject(Id = GameConstants.MENU_SCENE)]
            AssetReference menuSceneReference,
            [Inject(Id = GameConstants.LEVEL_SCENE)]
            AssetReference levelSceneReference
        )
        {
            _modalCreatorService = modalCreatorService;
            _saveLoad = saveLoad;
            _sceneSceneLoader = sceneSceneLoader;

            _gameDatabase = gameDatabase;
            _upgradeDatabase = upgradeDatabase;
            _weaponDatabase = weaponDatabase;
            _enemyDatabase = enemyDatabase;
            _towerDatabase = towerDatabase;
            
            _menuSceneReference = menuSceneReference;
            _levelSceneReference = levelSceneReference;
        }

        public Queue<ILoadingOperation> StartGamePipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new LoadPlayerProgressOperation(_saveLoad),
                new LoadDatabaseOperation(new List<UniTask> { _upgradeDatabase.LoadConfigs() }, "Load upgrades"),
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
                    _enemyDatabase.LoadConfigs(),
                    _weaponDatabase.LoadConfigs(),
                    _towerDatabase.LoadConfigs(),
                    _gameDatabase.LoadConfigs(),
                }, "Load configs"),
                new LoadDatabaseOperation(new List<UniTask>
                {
                    _towerDatabase.LoadPrefabs(),
                    _weaponDatabase.LoadPrefabs(),
                    _enemyDatabase.LoadPrefabs()
                }, "Load prefabs"),
                new LoadSceneOperation(
                    _sceneSceneLoader, 
                    _levelSceneReference.RuntimeKey.ToString(), 
                    _modalCreatorService.CloseModal)
            });
        }
        
        public Queue<ILoadingOperation> RestartLevelPipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new ReloadSceneOperation(_sceneSceneLoader, _modalCreatorService.CloseModal)
            });
        }

        public Queue<ILoadingOperation> BackToMenuFromLevelPipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new LoadDatabaseOperation(new List<UniTask>
                {
                    _enemyDatabase.UnloadConfigs(),
                    _weaponDatabase.UnloadConfigs(),
                    _towerDatabase.UnloadConfigs(),
                    _gameDatabase.UnloadConfigs(),
                }, "Unload configs"),
                new LoadDatabaseOperation(new List<UniTask>
                {
                    _towerDatabase.UnloadPrefabs(),
                    _weaponDatabase.UnloadPrefabs(),
                    _enemyDatabase.UnloadPrefabs()
                }, "Unload prefabs"),
                new LoadSceneOperation(
                    _sceneSceneLoader, 
                    _menuSceneReference.RuntimeKey.ToString(), 
                    () => _modalCreatorService.OpenModal(ModalType.Menu).Forget())
            });
        }
    }
}