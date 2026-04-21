using System;
using _Project.Scripts.Database.Game;

namespace _Project.Scripts.UI.WaveCounter
{
    public class WaveCounterModel
    {
        public event Action<int> OnTickTimer;
        public event Action OnEndTimer;

        private readonly int _timeBetweenWaves;
        private int _currentTimeBetweenWaves;
        
        public int CurrentTimeBetweenWaves
        {
            get => _currentTimeBetweenWaves;
            private set {
                _currentTimeBetweenWaves = value;
                OnTickTimer?.Invoke(_currentTimeBetweenWaves);
                
                if(value <= 0)
                    OnEndTimer?.Invoke();
            }
        }

        public WaveCounterModel(GameConfigDatabase gameConfigDatabase) => _timeBetweenWaves = gameConfigDatabase.GameConfig.TimeBetweenWaves;

        public void TickTimer() => CurrentTimeBetweenWaves--;

        public void ResetTimer() => CurrentTimeBetweenWaves = _timeBetweenWaves;
    }
}