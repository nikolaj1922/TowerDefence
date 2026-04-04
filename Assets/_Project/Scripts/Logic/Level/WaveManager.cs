using System;
using Zenject;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks; 
using _Project.Scripts.Enemy;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.Logic.Level
{
    public class WaveManager : ITickable
    {
        public event Action<int> OnWaveTimerStart;
        public event Action OnCompleteLevel;
        public event Action<int> OnCompleteWave;

        private AnalyticsService _analyticsService;
        private HealthModel _castleHealthModel;
        private EnemyFactory _enemyFactory;
        private GameRepository _gameRepository;
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
            [Inject(Id = GameConstants.CASTLE_HEALTH_MODEL_INJECT_ID)] HealthModel healthModel,
            AnalyticsService analyticsService
        )
        {
            _analyticsService = analyticsService;
            _castleHealthModel = healthModel;
            _gameRepository = gameRepository;
            _enemyFactory = enemyFactory;
        }

        public void Tick()
        {
            if (_castleHealthModel.CurrentHealth > 0)
                return;

            ResetWaveToken();
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
            (CurrentWave) * _gameRepository.GameConfig.coinsPerWave
            + TotalEnemyKilled * _gameRepository.GameConfig.coinsPerKill;
        
        private Wave GetNextWave() => 
            _waveIndex < _gameRepository.GameConfig.waves.Length 
                ? _gameRepository.GameConfig.waves[_waveIndex]
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