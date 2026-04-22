using System;
using Zenject;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory;
using Cysharp.Threading.Tasks;


namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModalPresenter: IInitializable, IDisposable
    {
        private ISaveLoad _saveLoad;
        private IAnalyticsService _analyticsService;
        private EndGameModalView _view;
        private LoadingCurtainPresenter _loadingCurtainPresenter;
        private ILoadingPipelineFactory _loadingPipelineFactory;

        [Inject]
        public void Construct(
            EndGameModalView view,
            IAnalyticsService analyticsService,
            ISaveLoad saveLoad,
            LoadingCurtainPresenter loadingCurtainPresenter,
            ILoadingPipelineFactory loadingPipelineFactory
        )
        {
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
        }
        
        public void Dispose()
        {
            _view.OnGoToMenuButtonClicked -= OnGoToMenuButtonClick;
            _view.OnTryAgainButtonClicked -= OnTryAgainButtonClick;
        }
        
        private void OnTryAgainButtonClick()
        {
            _analyticsService.SessionRestarted(_view.CurrentWave);
            
            _loadingCurtainPresenter
                .StartLoadingOperations(_loadingPipelineFactory.RestartLevelPipeline())
                .Forget();
        }

        private void OnGoToMenuButtonClick()
        {
            _analyticsService.ReturnedToMenu(
                _view.CurrentWave,
                _saveLoad.PlayerProgress.MetaCoinsCount);
            
            _loadingCurtainPresenter
                .StartLoadingOperations(_loadingPipelineFactory.BackToMenuFromLevelPipeline())
                .Forget();
        }
    }
}