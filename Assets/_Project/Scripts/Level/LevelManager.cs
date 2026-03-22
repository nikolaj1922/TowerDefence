using Zenject;
using UnityEngine;
using System.Collections;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.TowersDatabase;
using _Project.Scripts.Enemy;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.Repositories;
using _Project.Scripts.Infrastructure.CoroutineRunner;
using _Project.Scripts.UI.HealthBar;
using _Project.Scripts.Tower;

namespace _Project.Scripts.Level
{
    public class LevelManager : IInitializable
    {
        [Inject] private DiContainer _container;
        private TowerPrefabsDatabase _towerPrefabsDatabase;
        private EnemyFactory _enemyFactory;
        private EnemySpawner _enemySpawner;
        private CoroutineRunner _coroutineRunner;
        private LevelRepository _levelRepository;
        private TowersRepository _towersRepository;
        private EnemyConfigsRepository _enemyConfigsRepository;
        private DefeatModal _defeatModal;
        
        private int _wayIndex = 0;
        private int _enemyDied = 0;
        private int _totalEnemyOnWay = 0;
        
        [Inject]
        private void Construct(
            TowerPrefabsDatabase towerPrefabsDatabase,
            EnemyFactory enemyFactory,
            CoroutineRunner coroutineRunner,
            TowersRepository towersRepository,
            LevelRepository levelRepository,
            DefeatModal defeatModal)
        {
            _towerPrefabsDatabase = towerPrefabsDatabase;
            _levelRepository = levelRepository;
            _towersRepository = towersRepository;
            _enemyFactory = enemyFactory;
            _coroutineRunner = coroutineRunner;
            _defeatModal = defeatModal;
        }

        public void Initialize()
        {
            InitializeCastle();
            // _coroutineRunner.Run(StartWay(GetNextWay()));
        }

        private void InitializeCastle()
        {
            GameObject castleObject = _container.InstantiatePrefab(_towerPrefabsDatabase.Get(TowerType.Castle));
            CastleController castle = castleObject.GetComponent<CastleController>();
            castle.OnCastleDestroy += GameOver;
        }

        
        private Way GetNextWay() => _wayIndex < _levelRepository.LevelConfig.ways.Length ? _levelRepository.LevelConfig.ways[_wayIndex] : null;

        private IEnumerator StartWay(Way way)
        {
            _totalEnemyOnWay = way.enemies.Sum(e => e.enemyCount);
            
            yield return new WaitForSeconds(3f);
            
            
            foreach (var wayEnemyData in way.enemies)
            {
                for (int i = 0; i < wayEnemyData.enemyCount; i++)
                {
                    _enemyFactory.CreateEnemy(wayEnemyData.enemyType, onDeath: OnEnemyDeath);
                    yield return new WaitForSeconds(way.spawnFrequency);   
                }
            }
        }
        
        private void OnEnemyDeath()
        {
            _enemyDied++;
            
            if (_enemyDied == _totalEnemyOnWay)
            {
                _wayIndex++;
                
                Way way = GetNextWay();
                
                if (way == null)
                {
                    Debug.Log("Level completed");
                    return;
                }
                
                _coroutineRunner.Run(StartWay(GetNextWay()));
            }
        }

        private void GameOver()
        {
            var modal = _container.InstantiatePrefabForComponent<DefeatModal>(_defeatModal.gameObject);
            modal.Initialize();
        }
    }
}
