using Zenject;
using UnityEngine;
using System.Linq;
using System.Collections;
using _Project.Scripts.Tower;
using _Project.Scripts.Enemy;
using _Project.Scripts.Weapon;
using _Project.Scripts.Configs;
using _Project.Scripts.Tower.Castle;
using _Project.Scripts.Repositories;
using _Project.Scripts.Database.TowersDatabase;
using _Project.Scripts.Infrastructure.CoroutineRunner;

namespace _Project.Scripts.Level
{
    public class LevelManager : IInitializable
    {
        [Inject] private DiContainer _container;
        private EnemyFactory _enemyFactory;
        private TowerFactory _towerFactory;
        private WeaponFactory _weaponFactory;
        private CoroutineRunner _coroutineRunner;
        private LevelRepository _levelRepository;
        private TowerConfigsRepository _towerConfigsRepository;
        private DefeatModal _defeatModal;
        
        private int _wayIndex = 0;
        private int _enemyDied = 0;
        private int _totalEnemyOnWay = 0;
        
        [Inject]
        private void Construct(
            TowerPrefabsDatabase towerPrefabsDatabase,
            EnemyFactory enemyFactory,
            TowerFactory towerFactory,
            WeaponFactory weaponFactory,
            CoroutineRunner coroutineRunner,
            LevelRepository levelRepository,
            TowerConfigsRepository towerConfigsRepository,
            DefeatModal defeatModal)
        {
            _towerConfigsRepository = towerConfigsRepository;
            _weaponFactory = weaponFactory;
            _towerFactory = towerFactory;
            _levelRepository = levelRepository;
            _enemyFactory = enemyFactory;
            _coroutineRunner = coroutineRunner;
            _defeatModal = defeatModal;
        }

        public void Initialize()
        {
           Castle castle = (Castle)CreateTower(TowerType.Castle, _levelRepository.LevelConfig.castlePosition);
           castle.OnCastleDestroy += GameOver;
           StartNextWay();
        }

        private Tower.Tower CreateTower(TowerType towerType, Vector3 position)
        {
            Tower.Tower tower = _towerFactory.CreateTower(towerType, position);
            Weapon.Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerConfigsRepository.Get(towerType).weaponType, tower.WeaponPoint.transform.position, tower.WeaponPoint.transform);
            
            tower.SetWeapon(weapon);

            return tower;
        }
        
        private void StartNextWay() => _coroutineRunner.Run(StartWayRoutine(GetNextWay()));
        
        private Way GetNextWay() => _wayIndex < _levelRepository.LevelConfig.ways.Length ? _levelRepository.LevelConfig.ways[_wayIndex] : null;

        private IEnumerator StartWayRoutine(Way way)
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
            
            if (_enemyDied != _totalEnemyOnWay)
                return;
            
            _wayIndex++;
            
            if (GetNextWay() == null)
            { 
                CompleteLevel();
                return;
            }
            
            StartNextWay();
        }

        private void CompleteLevel()
        {
            Debug.Log("Level completed");
        }

        private void GameOver()
        {
            var modal = _container.InstantiatePrefabForComponent<DefeatModal>(_defeatModal.gameObject);
            modal.Initialize();
        }
    }
}
