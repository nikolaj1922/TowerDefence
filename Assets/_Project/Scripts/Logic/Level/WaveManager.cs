using System;
using Zenject;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks; 
using _Project.Scripts.Configs;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Enemies;
using _Project.Scripts.ConfigRepositories;

namespace _Project.Scripts.Logic.Level
{
    public class WaveManager
    {
        public event Action<int> OnWaveTimerStart;
        public event Action OnCompleteLevel;
        public event Action<int> OnCompleteWave;

        private EnemyFactory _enemyFactory;
        private GameRepository _gameRepository;
        private AnalyticsService _analyticsService;
        private int _waveIndex = 0;
        private int _enemyKilledOnWave = 0;
        private int _totalEnemiesOnWave = 0;
        
        private CancellationTokenSource _waveCancelToken;

        public int CurrentWave => _waveIndex + 1;
        public int TotalEnemyKilled { get; private set; }

        [Inject]
        private void Construct(
            EnemyFactory enemyFactory,
            GameRepository gameRepository,
            AnalyticsService analyticsService
        )
        {
            _gameRepository = gameRepository;
            _analyticsService = analyticsService;
            _enemyFactory = enemyFactory;
        }

        public void StartTimer(int waveCount) => OnWaveTimerStart?.Invoke(waveCount);

        public void StartNextWave()
        {
            UpdateWaveToken();
            Wave wave = GetNextWave();
            
            if (wave == null)
                return;

            StartWave(wave, _waveCancelToken.Token).Forget();
        }
        
        public int GetRewardForWaves() =>
            (CurrentWave) * _gameRepository.GameConfig.CoinsPerWave
            + TotalEnemyKilled * _gameRepository.GameConfig.CoinsPerKill;
        
        public void StopWave()
        {
            ResetWaveToken();
            _enemyFactory.StopActiveEnemies();
        }
        
        private Wave GetNextWave() => 
            _waveIndex < _gameRepository.GameConfig.Waves.Length 
                ? _gameRepository.GameConfig.Waves[_waveIndex]
                : null;

        private async UniTaskVoid StartWave(Wave wave, CancellationToken token)
        {
            _totalEnemiesOnWave = wave.enemyGroups.Sum(e => e.enemyCount);
            _enemyKilledOnWave = 0;
            
            _analyticsService.WaveStarted(CurrentWave, _totalEnemiesOnWave);
            
            foreach (var waveEnemyData in wave.enemyGroups)
            {
                for (int i = 0; i < waveEnemyData.enemyCount; i++)
                {
                    _enemyFactory.CreateEnemy(waveEnemyData.enemyType, onDeath: OnEnemyDeath);

                    await UniTask.WaitForSeconds(
                        wave.spawnFrequency,
                        false, 
                        PlayerLoopTiming.Update, 
                        token);
                }
            }
        }
        
        private void OnEnemyDeath()
        {
            _enemyKilledOnWave++;
            TotalEnemyKilled++;
            
            if (_enemyKilledOnWave != _totalEnemiesOnWave)
                return;
            
            OnCompleteWave?.Invoke(CurrentWave);
            
            _waveIndex++;

            TryStartNewWave();
        }
        
        private void TryStartNewWave()
        {
            if (GetNextWave() == null)
            {
                OnCompleteLevel?.Invoke();
                return;
            }

            StartTimer(CurrentWave);
        }
        
        private void ResetWaveToken()
        {
            _waveCancelToken?.Cancel();
            _waveCancelToken?.Dispose();
            _waveCancelToken = null;
        }

        private void UpdateWaveToken()
        {
            _waveCancelToken?.Cancel();
            _waveCancelToken?.Dispose();
            _waveCancelToken = new CancellationTokenSource();
        }
    }
}