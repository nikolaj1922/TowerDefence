using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.UI;

namespace _Project.Scripts.Logic.Level
{
    public class EndGameService
    {
        private readonly UIFactory _uiFactory;
        private readonly WaveManager _waveManager;
        private readonly ISaveLoad _saveLoad;

        public EndGameService(UIFactory uiFactory, WaveManager waveManager, ISaveLoad saveLoad)
        {
            _uiFactory = uiFactory;
            _waveManager = waveManager;
            _saveLoad = saveLoad;
        }
        
        public void GameOver() => EndLevel("Defeat!");
        
        public void GameVictory() => EndLevel("Victory!");
        
        private void EndLevel(string headerText)
        {
            int metaAdded = _waveManager.GetRewardForWaves();
            _saveLoad.AddMetaCoins(metaAdded);
            _uiFactory.CreateEndGameModal(metaAdded, headerText);
        }

    }
}