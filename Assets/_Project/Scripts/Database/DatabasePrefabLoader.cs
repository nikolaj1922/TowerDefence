using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Database
{
    public class DatabasePrefabLoader<TKey, TValue>
    {
        private Dictionary<TKey, AsyncOperationHandle<GameObject>> _handles;
        public Dictionary<TKey, TValue> Prefabs { get; private set; }
      
        
        public async UniTask LoadAssets(
            IEnumerable<(TKey key, AssetReferenceGameObject reference)> entries,
            Func<GameObject, TValue> map
            )
        {
            _handles = new Dictionary<TKey, AsyncOperationHandle<GameObject>>();
            Prefabs = new Dictionary<TKey, TValue>();

            List<UniTask> tasks = new();
            
            foreach (var (key, reference) in entries)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(reference);
                _handles[key] = handle;
                
                tasks.Add(handle.ToUniTask());
            }
            
            await UniTask.WhenAll(tasks);

            foreach (var (key, handle) in _handles)
                Prefabs[key] = map(handle.Result);
        }
        
        public void UnloadAssets()
        {
            if (_handles == null)
                return;

            foreach (var handle in _handles.Values)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
            
            _handles?.Clear();
            Prefabs?.Clear();

            _handles = null;
            Prefabs = null;
        }
    }
}