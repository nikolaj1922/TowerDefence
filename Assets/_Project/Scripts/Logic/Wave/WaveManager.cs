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
       public event Action<int> OnCompleteWave; 
       public event Action OnCompleteLevel;
       
       private readonly IEnemyFactory _enemyFactory; 
       private readonly IAnalyticsService _analytics; 
       private readonly WaveDTO[] _waves;
       
       private CancellationTokenSource _cts; 
       private int _waveIndex; 
       private int _killed; 
       private int _total;
       
       public int CurrentWave => _waveIndex + 1; 
       public int TotalEnemyKilled { get; private set; }

        public WaveManager(
            WavesDatabase database,
            IEnemyFactory enemyFactory,
            IAnalyticsService analytics)
        {
            _waves = database.GetConfig();
            _enemyFactory = enemyFactory;
            _analytics = analytics;
        } 
        public void StartTimer(int wave) => OnWaveTimerStart?.Invoke(wave);

        public void StopWave()
        {
            ResetToken();
            _enemyFactory.StopActiveEnemies();
        }

        public void StartWave()
        {
            if (_waveIndex >= _waves.Length) return;

            ResetToken();
            _cts = new CancellationTokenSource();

            SpawnWave(_waves[_waveIndex], _cts.Token).Forget();
        }

        private async UniTaskVoid SpawnWave(WaveDTO wave, CancellationToken token)
        {
            _enemyFactory.DespawnAllEnemies();

            _killed = 0;
            _total = wave.enemyGroups.Sum(x => x.enemyCount);

            _analytics.WaveStarted(CurrentWave, _total);

            foreach (var group in wave.enemyGroups)
            {
                for (int i = 0; i < group.enemyCount; i++)
                {
                    _enemyFactory.CreateEnemy(group.enemyType, OnEnemyDeath);
                    await UniTask.Delay(TimeSpan.FromSeconds(wave.spawnFrequency), cancellationToken: token);
                }
            }
        }

        private void OnEnemyDeath()
        {
            _killed++;
            TotalEnemyKilled++;

            if (_killed < _total) return;

            OnCompleteWave?.Invoke(CurrentWave);
            _waveIndex++;

            if (_waveIndex >= _waves.Length)
                OnCompleteLevel?.Invoke();
            else
                StartTimer(CurrentWave);
        }

        private void ResetToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
   }
}