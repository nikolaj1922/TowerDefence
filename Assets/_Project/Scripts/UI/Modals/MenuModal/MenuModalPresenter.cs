using System;
using Zenject;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.SceneLoader;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModalPresenter: IInitializable, IDisposable
    {
        private readonly MenuModalView _menuModalView;
        private readonly ISceneLoader _sceneLoader;
        private readonly IModalCreatorService _modalCreatorService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ISaveLoad _saveLoad;
        
        public MenuModalPresenter(
            MenuModalView menuModalView,
            IAnalyticsService analyticsService,
            ISceneLoader sceneLoader,
            IModalCreatorService modalCreatorService,
            ISaveLoad saveLoad
        )
        {
            _saveLoad = saveLoad;
            _analyticsService = analyticsService;
            _menuModalView = menuModalView;
            _sceneLoader = sceneLoader;
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
            _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE, _modalCreatorService.CloseModal).Forget();
        }
            

        private void OnOpenUpgradesClick() => _modalCreatorService.OpenModal(ModalType.Upgrades);
    }
}