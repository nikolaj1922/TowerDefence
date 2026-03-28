using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.SceneLoader
{
    public class SceneLoader
    {
        public async UniTask LoadScene(string sceneName, Action onLoadComplete = null)
        {
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            onLoadComplete?.Invoke();
        }
    }
}