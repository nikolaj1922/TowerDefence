using System;
using _Project.Scripts.Database.Modals;
using Zenject;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModalPresenter: IInitializable, IDisposable
    {
        private readonly MenuModalView _menuModalView;
        private readonly IModalCreatorService _modalCreatorService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ISaveLoad _saveLoad;
        private readonly ILoadingPipelineFactory _loadingPipelineFactory;
        private readonly ILoadingCurtainFactory _loadingCurtainFactory;
        
        public MenuModalPresenter(
            MenuModalView menuModalView,
            IAnalyticsService analyticsService,
            IModalCreatorService modalCreatorService,
            ISaveLoad saveLoad,
            ILoadingPipelineFactory loadingPipelineFactory,
            ILoadingCurtainFactory loadingCurtainFactory
        )
        {
            _loadingPipelineFactory = loadingPipelineFactory;
            _loadingCurtainFactory = loadingCurtainFactory;
            _saveLoad = saveLoad;
            _analyticsService = analyticsService;
            _menuModalView = menuModalView;
            _modalCreatorService = modalCreatorService;
        }
        
        public void Initialize()
        {
            _menuModalView.OnStartClicked += OnStartClick;
            _menuModalView.OnOpenUpgradesClicked += OnOpenUpgradesClick;
        }

        public void Dispose()
        {
            _menuModalView.OnStartClicked -= OnStartClick;
            _menuModalView.OnOpenUpgradesClicked -= OnOpenUpgradesClick;
        }
        
        private void OnStartClick()
        {
            _analyticsService.GameStarted(_saveLoad.PlayerProgress.MetaCoinsCount);
            _loadingCurtainFactory.Create(_loadingPipelineFactory.LevelPipeline()).Forget();
        }
        
        private void OnOpenUpgradesClick() => _modalCreatorService.OpenModal(ModalType.Upgrades);
    }
}