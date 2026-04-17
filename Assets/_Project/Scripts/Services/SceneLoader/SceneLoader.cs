using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadScene(string sceneName, Action onLoadComplete = null)
        {
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            onLoadComplete?.Invoke();
        }
    }
}