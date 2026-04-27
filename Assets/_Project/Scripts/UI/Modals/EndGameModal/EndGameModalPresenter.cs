using System;
using Zenject;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Services.Ads;
using _Project.Scripts.Services.GameSession;
using _Project.Scripts.Services.ModalCreator;
using Cysharp.Threading.Tasks;


namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModalPresenter: IInitializable, IDisposable
    {
        private readonly ISaveLoad _saveLoad;
        private readonly IGameSessionService _gameSessionService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IModalCreatorService  _modalCreatorService;
        private readonly EndGameModalView _view;
        private readonly LoadingCurtainPresenter _loadingCurtainPresenter;
        private readonly ILoadingPipelineFactory _loadingPipelineFactory;
        private readonly IAdsService _adsService;
        private readonly IRewardService _rewardService;

        public EndGameModalPresenter(
            IRewardService rewardService,
            IAdsService adsService,
            IGameSessionService gameSessionService,
            EndGameModalView view,
            IModalCreatorService modalCreatorService,
            IAnalyticsService analyticsService,
            ISaveLoad saveLoad,
            LoadingCurtainPresenter loadingCurtainPresenter,
            ILoadingPipelineFactory loadingPipelineFactory
        )
        {
            _rewardService = rewardService;
            _adsService = adsService;
            _gameSessionService = gameSessionService;
            _modalCreatorService = modalCreatorService;
            _loadingCurtainPresenter = loadingCurtainPresenter;
            _loadingPipelineFactory = loadingPipelineFactory;
            _view = view;
            _saveLoad = saveLoad;
            _analyticsService = analyticsService;
        }
        
        public void Initialize()
        {
            _view.OnGoToMenuButtonClicked += OnGoToMenuButtonClick;
            _view.OnTryAgainButtonClicked += OnTryAgainButtonClick;
            _adsService.OnInterstitialAdWatched += GoToMenuScene;
        }
        
        public void Dispose()
        {
            _view.OnGoToMenuButtonClicked -= OnGoToMenuButtonClick;
            _view.OnTryAgainButtonClicked -= OnTryAgainButtonClick;
            _adsService.OnInterstitialAdWatched -= GoToMenuScene;
        }
        
        private void OnTryAgainButtonClick()
        {
            _analyticsService.SessionRestarted(_view.CurrentWave);
            
            _modalCreatorService.CloseModal();
            
            _loadingCurtainPresenter
                .StartLoadingOperations(_loadingPipelineFactory.RestartLevelPipeline())
                .Forget();
        }

        private void OnGoToMenuButtonClick()
        {
            SaveReward();
            
            _analyticsService.ReturnedToMenu(
                _view.CurrentWave,
                _saveLoad.PlayerProgress.MetaCoinsCount);
            _gameSessionService.LevelToMenuTransition();

            if (_adsService.CanShowInterstitialAdOnTransitionToMenu(_gameSessionService.FromLevelToMenuTransitionCount))
            {
                _adsService.ShowInterstitialAd();
                return;
            }

            GoToMenuScene();
        }
        
        private void GoToMenuScene() {
            _modalCreatorService.CloseModal();
            _loadingCurtainPresenter
                .StartLoadingOperations(_loadingPipelineFactory.BackToMenuFromLevelPipeline())
                .Forget();
        }
        
        private void SaveReward()
        {
            _saveLoad.PlayerProgress.AddMetaCoins(_rewardService.GetReward());
            _saveLoad.SaveProgress();
        }
    }
}