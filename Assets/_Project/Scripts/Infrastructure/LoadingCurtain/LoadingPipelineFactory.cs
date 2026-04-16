using System.Collections.Generic;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;
using _Project.Scripts.Infrastructure.LoadingScene;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingPipelineFactory
    {
        private readonly ISaveLoad _saveLoad;
        private readonly SceneLoader _sceneSceneLoader;
        private readonly GameRepository _gameRepository;
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly TowerConfigsRepository _towerConfigsRepository;
        private readonly WeaponConfigsRepository _weaponConfigsRepository;
        private readonly ModalCreatorService _modalCreatorService;
        
        public LoadingPipelineFactory(
            ModalCreatorService modalCreatorService,
            ISaveLoad saveLoad, 
            SceneLoader sceneSceneLoader,
            GameRepository gameRepository,
            TowerConfigsRepository towerConfigsRepository,
            EnemyConfigsRepository enemyConfigsRepository,
            WeaponConfigsRepository weaponConfigsRepository
            )
        {
            _modalCreatorService = modalCreatorService;
            _saveLoad = saveLoad;
            _sceneSceneLoader = sceneSceneLoader;
            _towerConfigsRepository = towerConfigsRepository;
            _gameRepository = gameRepository;
            _enemyConfigsRepository = enemyConfigsRepository;
            _weaponConfigsRepository = weaponConfigsRepository;
        }
        
        public Queue<ILoadingOperation> GetStartGamePipeline()
        {
            return new Queue<ILoadingOperation>(new ILoadingOperation[]
            {
                new LoadPlayerProgressOperation(_saveLoad),
                // new LoadConfigsOperation(
                //     _weaponConfigsRepository,
                //     _enemyConfigsRepository,
                //     _towerConfigsRepository,
                //     _gameRepository),
                new LoadMenuSceneOperation(_sceneSceneLoader, _modalCreatorService),
            });
        } 
    }
}