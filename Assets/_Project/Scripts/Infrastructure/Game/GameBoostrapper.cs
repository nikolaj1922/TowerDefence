using Zenject;
using _Project.Scripts.Services.EnemiesRepository;
using _Project.Scripts.Services.LevelConfigRepository;

namespace _Project.Scripts.Infrastructure
{
    public class GameBoostrapper : IInitializable
    {
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly LevelConfigRepository _levelConfigsRepository;
        private readonly SceneLoader.SceneLoader _sceneLoader;
        
        public GameBoostrapper(
            EnemyConfigsRepository enemyConfigsRepository, 
            LevelConfigRepository levelConfigsRepository,
            SceneLoader.SceneLoader sceneLoader)
        {
            _enemyConfigsRepository = enemyConfigsRepository;
            _levelConfigsRepository = levelConfigsRepository;
            _sceneLoader = sceneLoader;
        }
        
        public void Initialize()
        {
            _levelConfigsRepository.Load();
            _enemyConfigsRepository.Load();
            _sceneLoader.LoadScene(GameConstants.MenuScene);
        }
    }
}