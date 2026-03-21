using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure.SceneLoader
{
    public class SceneLoader
    {
        private readonly CoroutineRunner.CoroutineRunner _routineRunner;

        public SceneLoader(CoroutineRunner.CoroutineRunner routineRunner)
        {
            _routineRunner = routineRunner;
        }
        
        public void LoadScene(string sceneName, Action onLoadComplete = null)
        {
            _routineRunner.Run(LoadSceneCoroutine(sceneName, onLoadComplete));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, Action onLoadComplete = null)
        {
            AsyncOperation wait = SceneManager.LoadSceneAsync(sceneName);

            if (wait.isDone == false)
                yield return null;
            
            onLoadComplete?.Invoke();
        }
    }
}