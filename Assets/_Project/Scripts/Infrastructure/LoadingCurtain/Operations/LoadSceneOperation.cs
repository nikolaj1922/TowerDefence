using System;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Services.SceneLoader;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadSceneOperation : ILoadingOperation
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly Action _onLoadComplete;
        private readonly string _sceneKey; 
        
        public string Description => "Load scene";

        public LoadSceneOperation(
            ISceneLoader sceneLoader, 
            string sceneKey,
            Action onLoadComplete = null)
        {
            _sceneKey = sceneKey;
            _onLoadComplete = onLoadComplete;
            _sceneLoader = sceneLoader;
        }

        public async UniTask Load() => await _sceneLoader.SwitchTo(_sceneKey, _onLoadComplete);
    }
}