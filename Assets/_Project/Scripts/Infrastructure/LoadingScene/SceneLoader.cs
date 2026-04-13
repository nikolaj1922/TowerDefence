using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.LoadingScene
{
    public class SceneLoader
    {
        public async UniTask LoadScene(string sceneName, Action onLoadComplete = null, IProgress<float> progress = null)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            
            if (operation == null)
            {
                Debug.LogError($"Failed to load scene: {sceneName}");
                return;
            }
            
            while (!operation.isDone)
            {
                progress?.Report(operation.progress);
                await UniTask.Yield();
            }
            
            progress?.Report(1f);
            
            onLoadComplete?.Invoke();
        }
    }
}