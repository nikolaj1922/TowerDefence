using Zenject;
using _Project.Scripts.Repositories;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly LevelRepository _levelRepository;
        private readonly SceneLoader.SceneLoader _sceneLoader;
        private readonly TowersRepository _towersRepository;
        
        public GameBoostrapper(
            TowersRepository towersRepository,
            EnemyConfigsRepository enemyConfigsRepository, 
            LevelRepository levelRepository,
            SceneLoader.SceneLoader sceneLoader)
        {
            _towersRepository = towersRepository;
            _enemyConfigsRepository = enemyConfigsRepository;
            _levelRepository = levelRepository;
            _sceneLoader = sceneLoader;
        }
        
        public void Initialize()
        {
            _towersRepository.Load();
            _levelRepository.Load();
            _enemyConfigsRepository.Load();
            _sceneLoader.LoadScene(GameConstants.MenuScene);
        }
    }
}