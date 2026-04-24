namespace _Project.Scripts.Services.GameSession
{
    public interface IGameSession
    {
        int FromLevelToMenuTransitionCount { get; }
        void LevelToMenuTransition();
    }
}