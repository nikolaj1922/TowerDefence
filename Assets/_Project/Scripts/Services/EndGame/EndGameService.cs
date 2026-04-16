using UnityEngine;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.UI.Modals.EndGameModal;
using _Project.Scripts.Database.ModalsPrefabDatabase;

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
            
            EndLevel("Defeat!").Forget();
        }
        
        public void GameVictory() => EndLevel("Victory!").Forget();
        
        private async UniTask EndLevel(string headerText)
        {
            int metaAdded = _waveManager.GetRewardForWaves();
            _saveLoad.AddMetaCoins(metaAdded);

            GameObject endGameModalObject = await _modalCreatorService.OpenModal(ModalType.EndGame);
            EndGameModalView endGameModal = endGameModalObject.GetComponent<EndGameModalView>();
            
            endGameModal.Initialize(headerText, metaAdded, _waveManager.CurrentWave);
        }
    }
}