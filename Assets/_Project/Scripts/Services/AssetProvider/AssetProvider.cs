using UnityEngine;
using System.Collections.Generic;

namespace _Project.Scripts.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, Object> _cache = new();

        public T Load<T>(string path) where T : Object
        {
            if (_cache.TryGetValue(path, out Object cached))
            {
                return cached as T;
            }

            T asset = Resources.Load<T>(path);

            if (asset == null)
            {
                Debug.LogError($"Asset not found at path: {path}");
                return null;
            }

            _cache[path] = asset;

            return asset;
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            T[] asset = Resources.LoadAll<T>(path);
            
            return asset;
        }
    }
}