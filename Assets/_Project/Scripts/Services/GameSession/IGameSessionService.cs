namespace _Project.Scripts.Services.GameSession
{
    public interface IGameSessionService
    {
        int FromLevelToMenuTransitionCount { get; }
        int TowerBuiltOnLevel { get; }
        void LevelToMenuTransition();
        void BuildTowerOnLevel();
        void ResetTowerOnLevel();
    }
}