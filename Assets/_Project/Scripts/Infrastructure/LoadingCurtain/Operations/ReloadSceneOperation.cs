using System;
using _Project.Scripts.Services.SceneLoader;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class ReloadSceneOperation : ILoadingOperation
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly Action _onLoadComplete;
        
        public string Description => "Load scene";

        public ReloadSceneOperation(ISceneLoader sceneLoader, Action onLoadComplete = null)
        {
            _onLoadComplete = onLoadComplete;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Load() => await _sceneLoader.Reload(_onLoadComplete);
    }
}