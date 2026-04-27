using System;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Services.AssetProvider
{
    public class AssetProviderService : IAssetProviderService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _cache = new();

        public async UniTask<T> Load<T>(AssetReference reference) where T : class
        {
            string key = reference.RuntimeKey.ToString();

            return await LoadByAddress<T>(key);
        }

        public async UniTask<T> LoadByAddress<T>(string address) where T : class
        {
            if (_cache.TryGetValue(address, out var handleOperation))
                return handleOperation.Result as T;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            await handle.Task;
            
            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load asset: {handle.Status}");
            
            _cache[address] = handle;

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
            ReleaseByAddress(key);
        }

        public void ReleaseByAddress(string address)
        {
            if (_cache.TryGetValue(address, out var handleOperation))
            {
                Addressables.Release(handleOperation);
                _cache.Remove(address);
            }
        }

        public void ReleaseInstance(GameObject reference) => Addressables.ReleaseInstance(reference);

        public void Clear()
        {
            foreach (var handle in _cache.Values)
                Addressables.Release(handle);

            _cache.Clear();
        }
    }
}