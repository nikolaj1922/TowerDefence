using System;
using _Project.Scripts.Services.SceneLoader;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class ReloadSceneOperation : ILoadingOperation
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly Action _onLoadComplete;
        
        public string Description => "Load scene";

        public ReloadSceneOperation(ISceneLoaderService sceneLoaderService, Action onLoadComplete = null)
        {
            _onLoadComplete = onLoadComplete;
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask Load() => await _sceneLoaderService.Reload(_onLoadComplete);
    }
}