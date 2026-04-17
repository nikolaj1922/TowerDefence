using System;

namespace _Project.Scripts.Logic.Wave
{
    public interface IWaveManager
    {
        event Action<int> OnWaveTimerStart;
        event Action OnCompleteLevel;
        event Action<int> OnCompleteWave;
        int CurrentWave { get; }
        int TotalEnemyKilled { get; }
        void StartTimer(int waveCount);
        void StartNextWave();
        int GetRewardForWaves();
        void StopWave();
    }
}