using Cysharp.Threading.Tasks;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingScene;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadMenuSceneOperation : ILoadingOperation
    {
        private readonly SceneLoader _sceneLoader;
        
        public string Description => "Load menu scene";

        public LoadMenuSceneOperation(SceneLoader sceneLoader) => _sceneLoader = sceneLoader;

        public async UniTask Load() => await _sceneLoader.LoadScene(GameConstants.MENU_SCENE);
    }
}