using System;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Modals.EndGameModal;
using _Project.Scripts.UI.TowerCreation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Logic.Level.Services
{
    public class LevelUIService : ILevelUIService, IInitializable
    {
        public event Action<int> OnCreateTower;
        
        private readonly IInstantiator _instantiator;
        private readonly UIFactory _uiFactory;
        private readonly IModalCreatorService _modalCreatorService;
        private readonly IWaveManager _waveManager;
        
        private CreateTowerPanelView _towerPanel;
        
        public LevelUIService(
            IInstantiator instantiator,
            UIFactory uiFactory, 
            IModalCreatorService modalCreatorService,
            IWaveManager waveManager)
        {
            _instantiator = instantiator;
            _waveManager = waveManager;
            _uiFactory = uiFactory;
            _modalCreatorService = modalCreatorService;
        }

        public void Initialize()
        {
            _uiFactory.CreateCoinCounterPanel();
            _uiFactory.CreateWaveCounterPanel();

            _towerPanel = _uiFactory.CreateTowerPanel(OnCreateTowerInternal);
        }
        
        public void ShowTowerPanel(Vector3 pos) => _towerPanel.ShowPanel(pos);
        
        public async UniTask ShowEndModal(string title)
        {
            var modal = await _modalCreatorService.OpenModal(ModalType.EndGame, _instantiator);
            var view = modal.GetComponent<EndGameModalView>();

            view.SetCurrentWave(_waveManager.CurrentWave);
            view.Draw(title, _waveManager.GetReward());
        }
        
        public void ShowContinueForAdsModal() => _modalCreatorService.OpenModal(ModalType.EndGameAds, _instantiator).Forget();

        private void OnCreateTowerInternal(int price) => OnCreateTower?.Invoke(price);
    }
}