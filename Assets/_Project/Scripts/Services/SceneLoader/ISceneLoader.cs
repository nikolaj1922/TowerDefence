using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SceneLoader
{
    public interface ISceneLoader
    {
        UniTask LoadScene(string sceneName, Action onLoadComplete = null);
    }
}