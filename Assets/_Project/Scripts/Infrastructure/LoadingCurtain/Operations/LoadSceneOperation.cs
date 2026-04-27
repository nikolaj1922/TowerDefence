using System;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Services.SceneLoader;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadSceneOperation : ILoadingOperation
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly Action _onLoadComplete;
        private readonly string _sceneKey; 
        
        public string Description => "Load scene";

        public LoadSceneOperation(
            ISceneLoaderService sceneLoaderService, 
            string sceneKey,
            Action onLoadComplete = null)
        {
            _sceneKey = sceneKey;
            _onLoadComplete = onLoadComplete;
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask Load() => await _sceneLoaderService.SwitchTo(_sceneKey, _onLoadComplete);
    }
}