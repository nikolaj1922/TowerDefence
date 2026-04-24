namespace _Project.Scripts.Services.GameSession
{
    public interface IGameSession
    {
        int FromLevelToMenuTransitionCount { get; }
        int TowerBuiltOnLevel { get; }
        void LevelToMenuTransition();
        void BuildTowerOnLevel();
        void ResetTowerOnLevel();
    }
}