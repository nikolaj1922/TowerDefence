using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, Object> _cache = new();

        public async UniTask<T> Load<T>(string path) where T : Object
        {
            if (_cache.TryGetValue(path, out Object cached))
            {
                return cached as T;
            }

            ResourceRequest request = Resources.LoadAsync<T>(path);
            await request;
            
            T asset = request.asset as T;

            if (asset == null)
            {
                Debug.LogError($"Asset not found at path: {path}");
                return null;
            }

            _cache[path] = asset;

            return asset;
        }

        public UniTask<T[]> LoadAll<T>(string path) where T : Object
        {
            T[] asset = Resources.LoadAll<T>(path);

            if (asset == null || asset.Length == 0)
            {
                Debug.LogError($"Asset not found at path: {path}");
                return UniTask.FromResult<T[]>(null);
            }

            return UniTask.FromResult(asset);
        }
    }
}