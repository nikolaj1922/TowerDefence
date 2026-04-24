using System;
using _Project.Scripts.Database.Modals;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Ads;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.UI.Modals.EndGameModal;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.UI.Modals.ContinueForAdsModal
{
    public class ContinueForAdsModalPresenter: IInitializable, IDisposable
    {
        private readonly IInstantiator _instantiator;
        private readonly ContinueForAdsModalView _view;
        private readonly IWaveManager _waveManager;
        private readonly IAdsService _adsService;
        private readonly IModalCreatorService _modalCreatorService;

        public ContinueForAdsModalPresenter(
            IInstantiator instantiator,
            IWaveManager waveManager,
            IAdsService adsService, 
            IModalCreatorService modalCreatorService,
            ContinueForAdsModalView view)
        {
            _instantiator = instantiator;
            _waveManager = waveManager;
            _modalCreatorService = modalCreatorService;
            _adsService = adsService;
            _view = view;
        }
        
        public void Initialize()
        {
            _view.OnWatchAdsClick += OnWatchAdsClick;
            _view.OnEndGameClick += OnEndGameClick;
        }

        public void Dispose()
        {
            _view.OnWatchAdsClick -= OnWatchAdsClick;
            _view.OnEndGameClick -= OnEndGameClick;
        }

        private void OnWatchAdsClick()
        {
            _adsService.ShowRewardedAd();
            _modalCreatorService.CloseModal();
        }

        private void OnEndGameClick() => CreateEndGameModal().Forget();
        
        private async UniTask CreateEndGameModal()
        {
            var modal = await _modalCreatorService.OpenModal(ModalType.EndGame, _instantiator);
            var view = modal.GetComponent<EndGameModalView>();

            view.SetCurrentWave(_waveManager.CurrentWave);
            view.Draw("Defeat!",_waveManager.GetReward());
        }
    }
}