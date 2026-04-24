using Zenject;

namespace _Project.Scripts.Services.GameSession
{
    public class GameSession : IGameSession, IInitializable
    {
        public int FromLevelToMenuTransitionCount { get; private set; }
        public int TowerBuiltOnLevel { get; private set; }

        public void Initialize()
        {
            FromLevelToMenuTransitionCount = 0;
            TowerBuiltOnLevel = 0;
        }

        public void BuildTowerOnLevel() => TowerBuiltOnLevel++;
        
        public void ResetTowerOnLevel() => TowerBuiltOnLevel = 0;
        
        public void LevelToMenuTransition() => FromLevelToMenuTransitionCount++;
    }
}