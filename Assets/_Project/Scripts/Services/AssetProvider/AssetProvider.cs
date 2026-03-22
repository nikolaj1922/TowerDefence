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

            if (asset == null)
            {
                Debug.LogError($"Asset not found at path: {path}");
                return null;
            }

            return asset;
        }
        
        public T Instantiate<T>(string path) where T : Object
        {
            if (_cache.TryGetValue(path, out var cached))
            {
                if (cached is T prefab)
                    return Object.Instantiate(prefab);
            }

            T loaded = Load<T>(path);
            _cache[path] = loaded;
            return Object.Instantiate(loaded);
        }

        public T Instantiate<T>(string path, Vector3 position) where T : Object
        {
            if (_cache.TryGetValue(path, out var cached))
            {
                if (cached is T prefab)
                    return Object.Instantiate(prefab, position, Quaternion.identity);
            }
            
            T loaded = Load<T>(path);
            _cache[path] = loaded;
            return Object.Instantiate(loaded, position, Quaternion.identity);
        }

        public T Instantiate<T>(string path, Transform parent) where T : Object
        {
            if (_cache.TryGetValue(path, out var cached))
            {
                if(cached is T prefab)
                    return Object.Instantiate(prefab, parent); 
            }
            
            T loaded = Load<T>(path);
            _cache[path] = loaded;
            return Object.Instantiate(loaded, parent);
        }
    }
}