using System;
using System.Linq;
using System.Threading;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.Waves;
using _Project.Scripts.Enemies;
using _Project.Scripts.Services.Analytics;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Logic.Wave
{
    public class WaveManager : IWaveManager
    {
        public event Action<int> OnWaveTimerStart;
        public event Action OnCompleteLevel;
        public event Action<int> OnCompleteWave;

        private readonly IEnemyFactory _enemyFactory;
        private readonly WavesDatabase _wavesDatabase;
        private readonly IAnalyticsService _analyticsService;
        private int _waveIndex;
        private int _enemyKilledOnWave;
        private int _totalEnemiesOnWave;
        
        private CancellationTokenSource _waveCancelToken;

        public int CurrentWave => _waveIndex + 1;
        public int TotalEnemyKilled { get; private set; }
        
        public WaveManager(
            WavesDatabase wavesDatabase,
            IEnemyFactory enemyFactory,
            IAnalyticsService analyticsService
        )
        {
            _wavesDatabase = wavesDatabase;
            _analyticsService = analyticsService;
            _enemyFactory = enemyFactory;
        }

        public void StartTimer(int waveCount) => OnWaveTimerStart?.Invoke(waveCount);
        
        public void StopWave()
        {
            ResetWaveToken();
            _enemyFactory.StopActiveEnemies();
        }
        
        public void StartWave()
        {
            UpdateWaveToken();
            WaveDTO wave = GetWave();
            
            if (wave == null)
                return;

            StartEnemySpawn(wave, _waveCancelToken.Token).Forget();
        }
        
        private WaveDTO GetWave() => 
            _waveIndex < _wavesDatabase.GetConfig().Length
                ? _wavesDatabase.GetConfig()[_waveIndex]
                : null;

        private async UniTaskVoid StartEnemySpawn(WaveDTO wave, CancellationToken token)
        {
            _enemyFactory.DespawnAllEnemies();
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
            if (GetWave() == null)
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