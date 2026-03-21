using UnityEngine;

namespace _Project.Scripts.Services
{
    public interface IAssetProvider
    {
        T Load<T>(string path) where T : Object;
        T[] LoadAll<T>(string path) where T : Object;
        T Instantiate<T>(string path) where T : Object;
        T Instantiate<T>(string path, Vector3 position) where T : Object;
        T Instantiate<T>(string path, Transform parent) where T : Object;
    }
}