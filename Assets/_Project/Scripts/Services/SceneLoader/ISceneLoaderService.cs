using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SceneLoader
{
    public interface ISceneLoaderService
    {
        UniTask SwitchTo(string sceneKey, Action onLoadComplete = null);
        UniTask Preload(string sceneKey);
        UniTask Reload(Action onLoadComplete = null);
    }
}