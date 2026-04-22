using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new();
        private string _currentSceneKey;
        
        public async UniTask SwitchTo(string sceneKey, Action onLoadComplete = null)
        {
            if (!_loadedScenes.ContainsKey(sceneKey))
                await Preload(sceneKey);
            
            var handle = _loadedScenes[sceneKey];
            await handle.Result.ActivateAsync().ToUniTask();
            
            SceneManager.SetActiveScene(handle.Result.Scene);
            
            onLoadComplete?.Invoke();
            
            _currentSceneKey = sceneKey;
            
            await UnloadAllExcept(sceneKey);
        }

        public async UniTask Reload(Action onLoadComplete = null)
        {
            if (_loadedScenes.TryGetValue(_currentSceneKey, out var handle))
            {
                await Addressables.UnloadSceneAsync(handle).ToUniTask();
                _loadedScenes.Remove(_currentSceneKey);
            }

            await SwitchTo(_currentSceneKey, onLoadComplete);
        }

        public async UniTask Preload(string sceneKey)
        {
            if (_loadedScenes.ContainsKey(sceneKey))
                return;

            var handle = Addressables.LoadSceneAsync(
                sceneKey,
                LoadSceneMode.Additive,
                activateOnLoad: false
            );
            
            await handle.ToUniTask();

            _loadedScenes[sceneKey] = handle;
        }

        private async UniTask UnloadAllExcept(string sceneKey)
        {
            List<string> keys = new(_loadedScenes.Keys);
            
            foreach (string key in keys)
            {
                if(key == sceneKey)
                    continue;
                
                var handle = _loadedScenes[key];

                await Addressables.UnloadSceneAsync(handle).ToUniTask();
                _loadedScenes.Remove(key);
            }
        }
    }
}