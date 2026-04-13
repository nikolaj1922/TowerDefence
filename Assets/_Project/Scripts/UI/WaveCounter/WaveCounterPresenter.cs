using System;
using System.Threading;
using _Project.Scripts.Logic.Wave;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.UI.WaveCounter
{
    public class WaveCounterPresenter: IInitializable, IDisposable
    {
        private const int WAVE_TIMER_TICK_SECONDS = 1;
        
        private readonly WaveCounterModel _counterModel;
        private readonly WaveCounterView _view;
        private readonly WaveManager _waveManager;
        
        private CancellationTokenSource _cancellationToken;
        
        public WaveCounterPresenter(
            WaveCounterModel counterModel, 
            WaveCounterView view, 
            WaveManager waveManager)
        {
            _view = view;
            _counterModel = counterModel;
            _waveManager = waveManager;
        }

        public void Initialize()
        {
            _waveManager.OnWaveTimerStart += ShowWavePanel;
            _counterModel.OnEndTimer += StartWave;
            _counterModel.OnTickTimer += _view.UpdateTimerText;
            _view.OnForceWaveClick += StartWave;
        }
        
        public void Dispose()
        {
            _waveManager.OnWaveTimerStart -= ShowWavePanel;
            _counterModel.OnEndTimer -= StartWave;
            _counterModel.OnTickTimer -= _view.UpdateTimerText;
            _view.OnForceWaveClick -= StartWave;
        }

        private void ShowWavePanel(int wayCount)
        {
            _view.ShowPanel(wayCount);
            _counterModel.ResetTimer();
            
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
            _cancellationToken = new CancellationTokenSource();
            
            StartWaveTimer(_cancellationToken.Token).Forget();
        }

        private void StartWave()
        {
            _view.HidePanel();
            _waveManager.StartNextWave();
            
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            _cancellationToken = null;
        }
        
        private async UniTaskVoid StartWaveTimer(CancellationToken token)
        {
            while (_counterModel.CurrentTimeBetweenWaves > 0)
            {
                _counterModel.TickTimer();
                await UniTask.WaitForSeconds(
                    WAVE_TIMER_TICK_SECONDS,
                    false,
                    PlayerLoopTiming.Update,
                    token);
            }
            
            _waveManager.StartNextWave();
        }
    }
}