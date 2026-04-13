using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.UI.Modals.EndGameModal;

namespace _Project.Scripts.Services.EndGame
{
    public class EndGameService
    {
        private readonly WaveManager _waveManager;
        private readonly ISaveLoad _saveLoad;
        private readonly AnalyticsService _analyticsService;
        private readonly ModalCreatorService _modalCreatorService;

        public EndGameService(
            WaveManager waveManager, 
            ISaveLoad saveLoad,
            AnalyticsService analyticsService,
            ModalCreatorService modalCreatorService
        )
        {
            _saveLoad = saveLoad;
            _modalCreatorService = modalCreatorService;
            _analyticsService = analyticsService;
            _waveManager = waveManager;
        }

        public void GameOver(int towersBuilt)
        {
            _analyticsService.GameOver(
                _waveManager.CurrentWave, 
                _waveManager.TotalEnemyKilled, 
                towersBuilt, 
                _waveManager.GetRewardForWaves());
            
            EndLevel("Defeat!");
        }
        
        public void GameVictory() => EndLevel("Victory!");
        
        private void EndLevel(string headerText)
        {
            int metaAdded = _waveManager.GetRewardForWaves();
            _saveLoad.AddMetaCoins(metaAdded);
            
            EndGameModal endGameModal =
                _modalCreatorService.OpenModal(ModalType.EndGame).GetComponent<EndGameModal>();
            endGameModal.SetMetaCoinText(metaAdded);
            endGameModal.SetHeaderText(headerText);
        }
    }
}