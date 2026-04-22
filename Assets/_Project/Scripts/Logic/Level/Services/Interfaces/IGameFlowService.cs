namespace _Project.Scripts.Logic.Level.Services.Interfaces
{
    public interface IGameFlowService
    {
        void StartLevel();
        void OnVictory();
        void OnDefeat(int towersBuilt);
    }
}