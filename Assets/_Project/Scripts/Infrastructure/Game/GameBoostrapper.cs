using Zenject;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.SceneLoader;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private readonly ISaveLoad _saveLoad;
        private readonly GameRepository _gameRepository;
        private readonly ISceneLoader _sceneLoader;
        private readonly WeaponConfigsRepository _weaponConfigsRepository;
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly TowerConfigsRepository _towerConfigsRepository;
        private readonly IModalCreatorService _modalCreatorService;

        public GameBoostrapper(
            TowerConfigsRepository towerConfigsRepository,
            EnemyConfigsRepository enemyConfigsRepository, 
            WeaponConfigsRepository weaponConfigsRepository,
            GameRepository gameRepository,
            ISaveLoad saveLoad,
            ISceneLoader sceneLoader,
            IModalCreatorService modalCreatorService)
        {
            _modalCreatorService = modalCreatorService;
            _saveLoad = saveLoad;
            _weaponConfigsRepository = weaponConfigsRepository;
            _towerConfigsRepository = towerConfigsRepository;
            _enemyConfigsRepository = enemyConfigsRepository;
            _gameRepository = gameRepository;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _saveLoad.LoadProgress();
            _gameRepository.Load();
            _towerConfigsRepository.Load();
            _enemyConfigsRepository.Load();
            _weaponConfigsRepository.Load();
            _sceneLoader.LoadScene(
                GameConstants.GameConstants.MENU_SCENE,
                () => _modalCreatorService.OpenModal(ModalType.Menu))
                .Forget();
        }
    }
}