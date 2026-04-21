namespace _Project.Scripts.Services.Analytics
{
    public interface IAnalyticsService
    {
        void GameStarted(int metaCurrencyTotal);
        void WaveStarted(int waveNumber, int enemiesToSpawn);
        void WaveCompleted(int waveNumber, int towersBuilt, int coinsRemaining);
        void TowerBuilt(int waveNumber, int coinSpent, int coinsRemaining, int towersTotal);
        void BuildRejected(string reason, int waveNumber);
        void CastleDamaged(int waveNumber, float castleHpRemaining);
        void GameOver(int wavesSurvived, int enemiesKilled, int towersBuilt, int metaEarned);
        void SessionRestarted(int wavesSurvived);
        void ReturnedToMenu(int wavesSurvived, int metaCurrencyTotal);
    }
}