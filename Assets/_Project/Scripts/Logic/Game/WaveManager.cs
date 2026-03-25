using Zenject;
using UnityEngine;
using System.Linq;
using System.Collections;
using _Project.Scripts.Enemy;
using _Project.Scripts.Configs;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.CoroutineRunner;
using _Project.Scripts.Logic.Health;

namespace _Project.Scripts.Logic.Game
{
    public class WaveManager : ITickable
    {
        [Inject] private DiContainer _container;
        private CoroutineRunner _coroutineRunner;
        private EnemyFactory _enemyFactory;
        private GameRepository _gameRepository;
        private HealthModel _castleHealthModel; 
        
        public int WaveIndex { get; private set; } = 0;
        public int TotalEnemyKilled { get; private set; }

        private int _enemyKilledOnWave = 0;
        private int _totalEnemyOnWave = 0;
        
        private Coroutine _waveRoutine;
        
        [Inject]
        private void Construct(
            EnemyFactory enemyFactory,
            CoroutineRunner coroutineRunner,
            GameRepository gameRepository,
            [Inject(Id = "CastleHealthModel")] HealthModel healthModel
        )
        {
            _castleHealthModel = healthModel;
            _gameRepository = gameRepository;
            _enemyFactory = enemyFactory;
            _coroutineRunner = coroutineRunner;
        }

        public void Tick()
        {
            if (_castleHealthModel.CurrentHealth <= 0 && _waveRoutine != null)
                _coroutineRunner.Stop(_waveRoutine);
        }

        public void StartNextWave() => _waveRoutine = _coroutineRunner.Run(StartWaveRoutine(GetNextWave()));
        
        private Wave GetNextWave() => WaveIndex < _gameRepository.GameConfig.waves.Length ? _gameRepository.GameConfig.waves[WaveIndex] : null;

        private IEnumerator StartWaveRoutine(Wave wave)
        {
            _enemyKilledOnWave = 0;
            _totalEnemyOnWave = wave.enemyGroups.Sum(e => e.enemyCount);
            
            yield return new WaitForSeconds(3f);
            
            foreach (var waveEnemyData in wave.enemyGroups)
            {
                for (int i = 0; i < waveEnemyData.enemyCount; i++)
                {
                    _enemyFactory.CreateEnemy(waveEnemyData.enemyType, onDeath: OnEnemyDeath);
                    yield return new WaitForSeconds(wave.spawnFrequency);   
                }
            }
        }
        
        private void OnEnemyDeath()
        {
            _enemyKilledOnWave++;
            TotalEnemyKilled++;
            
            if (_enemyKilledOnWave != _totalEnemyOnWave)
                return;
            
            WaveIndex++;
            
            if (GetNextWave() == null)
            { 
                CompleteLevel();
                return;
            }
            
            StartNextWave();
        }

        private void CompleteLevel()
        {
            Debug.Log("Level completed");
        }
    }
}