using Zenject;
using UnityEngine;
using System.Collections;
using _Project.Scripts.Enemy;
using _Project.Scripts.Tower;
using _Project.Scripts.Infrastructure.CoroutineRunner;
using _Project.Scripts.Services.EnemiesRepository;
using _Project.Scripts.Services.LevelConfigRepository;

namespace _Project.Scripts.Level
{
    public class LevelManager : IInitializable
    {
        [Inject] private DiContainer _container;
        private EnemyFactory _enemyFactory;
        private EnemySpawner _enemySpawner;
        private CoroutineRunner _coroutineRunner;
        private LevelConfigRepository _levelConfigRepository;
        private EnemyConfigsRepository _enemyConfigsRepository;

        private CastleController _castle;
        private DefeatModal _defeatModal;
        
        private int _wayIndex = 0;
        
        [Inject]
        private void Construct(
            EnemyFactory enemyFactory,
            CoroutineRunner coroutineRunner,
            LevelConfigRepository levelConfigRepository,
            CastleController castleController,
            DefeatModal defeatModal)
        {
            _castle = castleController;
            _levelConfigRepository = levelConfigRepository;
            _enemyFactory = enemyFactory;
            _coroutineRunner = coroutineRunner;
            _defeatModal = defeatModal;
        }

        public void Initialize()
        {
            _castle.InitHealth(_levelConfigRepository.LevelConfig.castleHealth);
            _castle.OnCastleDestroy += GameOver;
            _coroutineRunner.Run(StartWay());
        }
        
        private IEnumerator StartWay()
        {
            yield return new WaitForSeconds(3f);

            var currentWay = _levelConfigRepository.LevelConfig.ways[_wayIndex];
            
            foreach (var wayEnemyData in currentWay.enemies)
            {
                for (int i = 0; i < wayEnemyData.enemyCount; i++)
                {
                    _enemyFactory.CreateEnemy(wayEnemyData.enemyType);
                    yield return new WaitForSeconds(currentWay.spawnFrequency);   
                }
            }
        }

        private void GameOver()
        {
            var modal = _container.InstantiatePrefabForComponent<DefeatModal>(_defeatModal.gameObject);
            modal.Initialize();
        }
    }
}