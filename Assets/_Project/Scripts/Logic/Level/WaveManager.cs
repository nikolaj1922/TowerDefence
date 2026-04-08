using System;
using Zenject;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Enemies;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.Logic.Level
{
    public class WaveManager : IInitializable, IDisposable
    {
        public event Action<int> OnWaveTimerStart;
        public event Action OnCompleteLevel;
        
        private HealthModel _castleHealthModel;
        private EnemyFactory _enemyFactory;
        private GameRepository _gameRepository;
        private int _waveIndex = 0;
        private int _totalEnemyKilled = 0;
        private int _enemyKilledOnWave = 0;
        private int _totalEnemyOnWave = 0;

        private CancellationTokenSource _waveCancelToken;
        
        [Inject]
        private void Construct(
            EnemyFactory enemyFactory,
            GameRepository gameRepository,
            [Inject(Id = GameConstants.CASTLE_HEALTH_MODEL_INJECT_ID)] HealthModel healthModel
        )
        {
            _castleHealthModel = healthModel;
            _gameRepository = gameRepository;
            _enemyFactory = enemyFactory;
        }

        public void Initialize() => _castleHealthModel.OnDeath += StopWave;
        
        public void Dispose() => _castleHealthModel.OnDeath -= StopWave;

        public void StartTimer(int waveCount) => OnWaveTimerStart?.Invoke(waveCount);

        public void StartNextWave()
        {
            _waveCancelToken?.Cancel();
            _waveCancelToken?.Dispose();
            _waveCancelToken = new CancellationTokenSource();
            
            Wave wave = GetNextWave();
            
            if (wave == null)
                return;

            StartWave(wave, _waveCancelToken.Token).Forget();
        }
        
        public int GetRewardForWaves() =>
            (_waveIndex + 1) * _gameRepository.GameConfig.CoinsPerWave
            + _totalEnemyKilled * _gameRepository.GameConfig.CoinsPerKill;
        
        private void StopWave()
        {
            _waveCancelToken?.Cancel();
            _waveCancelToken?.Dispose();
            _waveCancelToken = null;
        }
        
        private Wave GetNextWave() => 
            _waveIndex < _gameRepository.GameConfig.Waves.Length 
                ? _gameRepository.GameConfig.Waves[_waveIndex]
                : null;

        private async UniTaskVoid StartWave(Wave wave, CancellationToken token)
        {
            _enemyKilledOnWave = 0;
            _totalEnemyOnWave = wave.enemyGroups.Sum(e => e.enemyCount);
            
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
            _totalEnemyKilled++;
            
            if (_enemyKilledOnWave != _totalEnemyOnWave)
                return;
            
            _waveIndex++;
            
            if (GetNextWave() == null)
            { 
                OnCompleteLevel?.Invoke();
                return;
            }
            
            StartTimer(_waveIndex + 1);
        }
    }
}