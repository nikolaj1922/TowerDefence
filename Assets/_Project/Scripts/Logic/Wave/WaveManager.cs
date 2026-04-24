using System;
using System.Linq;
using System.Threading;
using _Project.Scripts.Database.Game;
using _Project.Scripts.Enemies;
using _Project.Scripts.Services.Analytics;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Logic.Wave
{
    public class WaveManager : IWaveManager
    {
        public event Action<int> OnWaveTimerStart;
        public event Action OnCompleteLevel;
        public event Action<int> OnCompleteWave;

        private IEnemyFactory _enemyFactory;
        private GameDatabase _gameDatabase;
        private IAnalyticsService _analyticsService;
        private int _waveIndex;
        private int _enemyKilledOnWave;
        private int _totalEnemiesOnWave;
        
        private CancellationTokenSource _waveCancelToken;

        public int CurrentWave => _waveIndex + 1;
        public int TotalEnemyKilled { get; private set; }

        [Inject]
        private void Construct(
            IEnemyFactory enemyFactory,
            GameDatabase gameDatabase,
            IAnalyticsService analyticsService
        )
        {
            _gameDatabase = gameDatabase;
            _analyticsService = analyticsService;
            _enemyFactory = enemyFactory;
        }

        public void StartTimer(int waveCount) => OnWaveTimerStart?.Invoke(waveCount);

        public int GetReward() =>
            (CurrentWave) * _gameDatabase.GetConfig().CoinsPerWave
            + TotalEnemyKilled * _gameDatabase.GetConfig().CoinsPerKill;
        
        public void StopWave()
        {
            ResetWaveToken();
            _enemyFactory.StopActiveEnemies();
        }
        
        public void StartWave()
        {
            UpdateWaveToken();
            Configs.Wave wave = GetWave();
            
            if (wave == null)
                return;

            StartEnemySpawn(wave, _waveCancelToken.Token).Forget();
        }
        
        private Configs.Wave GetWave() => 
            _waveIndex < _gameDatabase.GetConfig().Waves.Length 
                ? _gameDatabase.GetConfig().Waves[_waveIndex]
                : null;

        private async UniTaskVoid StartEnemySpawn(Configs.Wave wave, CancellationToken token)
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