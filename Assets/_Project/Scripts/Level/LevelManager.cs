using Zenject;
using UnityEngine;
using System.Collections;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemy;
using _Project.Scripts.Tower;
using _Project.Scripts.Repositories;
using _Project.Scripts.Infrastructure.CoroutineRunner;

namespace _Project.Scripts.Level
{
    public class LevelManager : IInitializable
    {
        [Inject] private DiContainer _container;
        private EnemyFactory _enemyFactory;
        private EnemySpawner _enemySpawner;
        private CoroutineRunner _coroutineRunner;
        private LevelRepository _levelRepository;
        private TowersRepository _towersRepository;
        private EnemyConfigsRepository _enemyConfigsRepository;

        private CastleController _castle;
        private DefeatModal _defeatModal;
        
        private int _wayIndex = 0;
        
        [Inject]
        private void Construct(
            EnemyFactory enemyFactory,
            CoroutineRunner coroutineRunner,
            TowersRepository towersRepository,
            LevelRepository levelRepository,
            CastleController castleController,
            DefeatModal defeatModal)
        {
            _castle = castleController;
            _levelRepository = levelRepository;
            _towersRepository = towersRepository;
            _enemyFactory = enemyFactory;
            _coroutineRunner = coroutineRunner;
            _defeatModal = defeatModal;
        }

        public void Initialize()
        {
            InitializeCastle();
            _coroutineRunner.Run(StartWay());
        }

        private void InitializeCastle()
        {
            _castle.InitHealth(_levelRepository.LevelConfig.castleHealth);

            TowerConfig castleConfig = _towersRepository.Get(TowerType.Castle);
            
            TowerAim castleAim = _castle.GetComponent<TowerAim>();
            TowerAttack castleAttack = _castle.GetComponent<TowerAttack>();
            
            castleAim.Initialize(castleConfig.attackRange, castleConfig.rotationSpeed);
            castleAttack.Initialize(castleConfig.damage, castleConfig.attackSpeed);
            
            _castle.OnCastleDestroy += GameOver;
        }

        private IEnumerator StartWay()
        {
            yield return new WaitForSeconds(3f);

            var currentWay = _levelRepository.LevelConfig.ways[_wayIndex];
            
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