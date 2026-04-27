using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Database
{
    public class DatabaseConfigLoader<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Configs { get; private set; }

        private AsyncOperationHandle<IList<TValue>> _handle;
        
        public async UniTask LoadAssets(string label, Func<TValue, TKey> keySelector)
        {
            if (_handle.IsValid())
                return;
            
            _handle = Addressables.LoadAssetsAsync<TValue>(label);
         
            await _handle.ToUniTask();
            
            Configs = _handle.Result.ToArray().ToDictionary(keySelector,  x => x);
        }

        public void UnloadAssets()
        {
            if (!_handle.IsValid())
                return;

            Addressables.Release(_handle);
            _handle = default;

            Configs?.Clear();
            Configs = null;
        }
    }
}