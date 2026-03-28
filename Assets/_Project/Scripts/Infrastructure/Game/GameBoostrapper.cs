using Zenject;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.SaveLoad;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private readonly SceneLoader.SceneLoader _sceneLoader;
        private readonly GameRepository _gameRepository;
        private readonly WeaponConfigsRepository _weaponConfigsRepository;
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly TowerConfigsRepository _towerConfigsRepository;
        private readonly ISaveLoad _saveLoad;
        
        public GameBoostrapper(
            TowerConfigsRepository towerConfigsRepository,
            EnemyConfigsRepository enemyConfigsRepository, 
            WeaponConfigsRepository weaponConfigsRepository,
            GameRepository gameRepository,
            ISaveLoad saveLoad,
            SceneLoader.SceneLoader sceneLoader)
        {
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
            _sceneLoader.LoadScene(GameConstants.GameConstants.MENU_SCENE).Forget();
        }
    }
}