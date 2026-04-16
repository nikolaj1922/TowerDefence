using System;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Services.AssetProvider
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _cache = new();

        public async UniTask<T> Load<T>(AssetReference reference) where T : class
        {
            string key = reference.RuntimeKey.ToString();

            if (_cache.TryGetValue(key, out var handleOperation))
                return handleOperation.Result as T;
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load asset: {handle.Status}");
            
            _cache[key] = handle;

            return handle.Result;
        }

        public async UniTask<T[]> LoadByLabel<T>(string label)
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label);
            await handle.Task;
            
            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load asset: {handle.Status}");

            return handle.Result.ToArray();
        }

        public async UniTask<GameObject> Instantiate(AssetReferenceGameObject reference, Transform parent = null)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(reference, parent);
            await handle.Task;
            
            if(handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to instantiate asset: {reference.RuntimeKey}");

            return handle.Result;
        }

        public void Release(AssetReference reference)
        {
            string key = reference.RuntimeKey.ToString();

            if (_cache.TryGetValue(key, out var handleOperation))
            {
                Addressables.Release(handleOperation);
                _cache.Remove(key);
            }
        }

        public void ReleaseInstance(GameObject reference)
        {
            Addressables.ReleaseInstance(reference);
        }

        public void Clear()
        {
            foreach (var handle in _cache.Values)
                Addressables.Release(handle);

            _cache.Clear();
        }
    }
}