using System;
using Zenject;
using System.Threading;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.UI.WaveCounter;

namespace _Project.Scripts.Logic.StartNextWavePanel
{
    public class StartWavePresenter: IInitializable, IDisposable
    {
        private const int WAVE_TIMER_TICK_SECONDS = 1;
        
        private readonly StartWaveModel _model;
        private readonly WaveCounterPanel _view;
        private readonly WaveManager _waveManager;
        
        private CancellationTokenSource _cancellationToken;
        
        public StartWavePresenter(
            StartWaveModel model, 
            WaveCounterPanel view, 
            WaveManager waveManager)
        {
            _view = view;
            _model = model;
            _waveManager = waveManager;
        }

        public void Initialize()
        {
            _waveManager.OnWaveTimerStart += ShowWavePanel;
            _model.OnEndTimer += StartWave;
            _model.OnTickTimer += _view.UpdateTimerText;
            _view.OnForceWaveClick += StartWave;
        }
        
        public void Dispose()
        {
            _waveManager.OnWaveTimerStart -= ShowWavePanel;
            _model.OnEndTimer -= StartWave;
            _model.OnTickTimer -= _view.UpdateTimerText;
            _view.OnForceWaveClick -= StartWave;
        }

        private void ShowWavePanel(int wayCount)
        {
            _view.ShowPanel(wayCount);
            _model.ResetTimer();
            
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
            while (_model.CurrentTimeBetweenWaves > 0)
            {
                _model.TickTimer();
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