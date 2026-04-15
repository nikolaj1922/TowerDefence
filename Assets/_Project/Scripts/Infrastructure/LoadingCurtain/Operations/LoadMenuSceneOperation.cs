using _Project.Scripts.Database.ModalsPrefabDatabase;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingScene;
using _Project.Scripts.Services.ModalCreator;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadMenuSceneOperation : ILoadingOperation
    {
        private readonly SceneLoader _sceneLoader;
        private readonly ModalCreatorService _modalCreatorService;
        
        public string Description => "Load menu scene";

        public LoadMenuSceneOperation(SceneLoader sceneLoader, ModalCreatorService modalCreatorService)
        {
            _modalCreatorService = modalCreatorService;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Load() => 
            await _sceneLoader.LoadScene(GameConstants.MENU_SCENE, () => _modalCreatorService.OpenModal(ModalType.Menu));
    }
}