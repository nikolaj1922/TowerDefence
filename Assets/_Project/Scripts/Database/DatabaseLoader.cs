using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Database
{
    public class DatabaseLoader<TKey, TValue>
    {
        public LoadPrefabResult<TKey, TValue> Data { get; private set; }
        
        public async UniTask LoadPrefabs(
            IEnumerable<(TKey key, AssetReferenceGameObject reference)> entries,
            Func<GameObject, TValue> map
            )
        {
            Dictionary<TKey, TValue> result = new();
            Dictionary<TKey, AsyncOperationHandle<GameObject>> handles = new();

            List<UniTask> tasks = new();
            
            foreach (var (key, reference) in entries)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(reference);
                handles[key] = handle;
                
                tasks.Add(handle.ToUniTask());
            }
            
            await UniTask.WhenAll(tasks);

            foreach (var (key, handle) in handles)
                result[key] = map(handle.Result);
            
            Data = new LoadPrefabResult<TKey, TValue>(result, handles);
        }
        
        public void Unload()
        {
            if (Data.Handles == null)
                return;

            foreach (var handle in Data.Handles.Values)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
            
            Data.Handles.Clear();
            Data.Cache.Clear();
        }
    }
    
    public  class LoadPrefabResult<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Cache { get; }
        public Dictionary<TKey, AsyncOperationHandle<GameObject>> Handles { get; }

        public LoadPrefabResult(
            Dictionary<TKey, TValue> cache,
            Dictionary<TKey, AsyncOperationHandle<GameObject>> handles
        )
        {
            Cache = cache;
            Handles = handles;
        }
    }
}