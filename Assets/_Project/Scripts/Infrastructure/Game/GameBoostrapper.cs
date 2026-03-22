using Zenject;
using _Project.Scripts.Repositories;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private readonly SceneLoader.SceneLoader _sceneLoader;
        private readonly LevelRepository _levelRepository;
        private readonly WeaponConfigsRepository _weaponConfigsRepository;
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly TowerConfigsRepository _towerConfigsRepository;
        
        public GameBoostrapper(
            TowerConfigsRepository towerConfigsRepository,
            EnemyConfigsRepository enemyConfigsRepository, 
            WeaponConfigsRepository weaponConfigsRepository,
            LevelRepository levelRepository,
            SceneLoader.SceneLoader sceneLoader)
        {
            _weaponConfigsRepository = weaponConfigsRepository;
            _towerConfigsRepository = towerConfigsRepository;
            _enemyConfigsRepository = enemyConfigsRepository;
            _levelRepository = levelRepository;
            _sceneLoader = sceneLoader;
        }
        
        public void Initialize()
        {
            _levelRepository.Load();
            _towerConfigsRepository.Load();
            _enemyConfigsRepository.Load();
            _weaponConfigsRepository.Load();
            _sceneLoader.LoadScene(GameConstants.MenuScene);
        }
    }
}