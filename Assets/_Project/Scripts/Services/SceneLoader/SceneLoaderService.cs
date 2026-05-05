using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services.SceneLoader
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _loadedScenes = new();
        private string _currentSceneKey;
        
        public async UniTask SwitchTo(string sceneKey, Action onLoadComplete = null)
        {
            if (!_loadedScenes.ContainsKey(sceneKey))
                await Preload(sceneKey);
            
            AsyncOperationHandle<SceneInstance> handle = _loadedScenes[sceneKey];
            await handle.Result.ActivateAsync().ToUniTask();
            
            SceneManager.SetActiveScene(handle.Result.Scene);
            
            onLoadComplete?.Invoke();
            
            _currentSceneKey = sceneKey;
            
            await UnloadAllExcept(sceneKey);
        }

        public async UniTask Reload(Action onLoadComplete = null)
        {
            if (_loadedScenes.TryGetValue(_currentSceneKey, out AsyncOperationHandle<SceneInstance> handle))
            {
                await Addressables.UnloadSceneAsync(handle).ToUniTask();
                _loadedScenes.Remove(_currentSceneKey);
            }

            await SwitchTo(_currentSceneKey, onLoadComplete);
        }

        private async UniTask Preload(string sceneKey)
        {
            if (_loadedScenes.ContainsKey(sceneKey))
                return;

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(
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
                
                AsyncOperationHandle<SceneInstance> handle = _loadedScenes[key];

                await Addressables.UnloadSceneAsync(handle).ToUniTask();
                _loadedScenes.Remove(key);
            }
        }
    }
}