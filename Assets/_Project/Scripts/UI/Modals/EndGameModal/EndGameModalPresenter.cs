using System;
using System.Collections.Generic;
using _Project.Scripts.Database.Modals;
using Zenject;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;
using _Project.Scripts.Services.SceneLoader;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModalPresenter: IInitializable, IDisposable
    {
        private ISaveLoad _saveLoad;
        private ISceneLoader _sceneLoader;
        private IAnalyticsService _analyticsService;
        private IModalCreatorService _modalCreatorService;
        private EndGameModalView _view;
        private ILoadingCurtainFactory _loadingCurtainFactory;
        private ILoadingPipelineFactory _loadingPipelineFactory;
        
        private AssetReference _menuSceneReference;
        private AssetReference _levelSceneReference;

        [Inject]
        public void Construct(
            EndGameModalView view,
            ISceneLoader sceneLoader,
            IAnalyticsService analyticsService,
            ISaveLoad saveLoad,
            IModalCreatorService modalCreatorService,
            ILoadingCurtainFactory loadingCurtainFactory,
            ILoadingPipelineFactory loadingPipelineFactory,
            [Inject(Id = GameConstants.MENU_SCENE)]
            AssetReference menuSceneReference,
            [Inject(Id = GameConstants.LEVEL_SCENE)]
            AssetReference levelSceneReference
        )
        {
            _loadingCurtainFactory = loadingCurtainFactory;
            _loadingPipelineFactory = loadingPipelineFactory;
            _view = view;
            _modalCreatorService = modalCreatorService;
            _sceneLoader = sceneLoader;
            _saveLoad = saveLoad;
            _analyticsService = analyticsService;
            _menuSceneReference = menuSceneReference;
            _levelSceneReference = levelSceneReference;
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
            _loadingCurtainFactory.Create(_loadingPipelineFactory.RestartLevelPipeline());
        }

        private void OnGoToMenuButtonClick()
        {
            _analyticsService.ReturnedToMenu(
                _view.CurrentWave,
                _saveLoad.PlayerProgress.MetaCoinsCount);
            _loadingCurtainFactory.Create(_loadingPipelineFactory.RestartLevelPipeline());
            _sceneLoader
                .SwitchTo(
                    _menuSceneReference.RuntimeKey.ToString(),
                    () => _modalCreatorService.OpenModal(ModalType.Menu));
        }
    }
}