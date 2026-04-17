using System;
using Zenject;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Services.SceneLoader;

namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModalPresenter: IInitializable, IDisposable
    {
        private ISaveLoad _saveLoad;
        private ISceneLoader _sceneLoader;
        private IAnalyticsService _analyticsService;
        private IModalCreatorService _modalCreatorService;
        private EndGameModalView _view;

        [Inject]
        public void Construct(
            EndGameModalView view,
            ISceneLoader sceneLoader,
            IAnalyticsService analyticsService,
            ISaveLoad saveLoad,
            IModalCreatorService modalCreatorService
        )
        {
            _view = view;
            _modalCreatorService = modalCreatorService;
            _sceneLoader = sceneLoader;
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
            _sceneLoader
                .LoadScene(
                    GameConstants.LEVEL_SCENE,
                    _modalCreatorService.CloseModal)
                .Forget();
        }

        private void OnGoToMenuButtonClick()
        {
            _analyticsService.ReturnedToMenu(
                _view.CurrentWave,
                _saveLoad.PlayerProgress.MetaCoinsCount);
            _sceneLoader
                .LoadScene(
                    GameConstants.MENU_SCENE,
                    () => _modalCreatorService.OpenModal(ModalType.Menu))
                .Forget();
        }
    }
}