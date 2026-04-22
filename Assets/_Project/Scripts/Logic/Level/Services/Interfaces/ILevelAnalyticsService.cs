namespace _Project.Scripts.Logic.Level.Services.Interfaces
{
    public interface ILevelAnalyticsService
    {
        void OnTowerBuilt(int coinPrice, int totalTowers);
        void OnWaveCompleted(int wave, int towersBuilt);
        void OnCastleDamaged(float hp);
        void GameOver(int towersBuilt);
    }
}