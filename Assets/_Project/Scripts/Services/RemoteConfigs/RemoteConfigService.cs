using System;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Services.RemoteConfigs
{
    public class RemoteConfigService: IRemoteConfigService
    {
        private bool _isInitialized;
        
        public bool TryGetConfig<T>(string key, out T result)
        {
            result = default;

            if (_isInitialized == false)
            {
                Debug.LogError("Remote config not initialized");
                return false;
            }

            try
            {
                string json = FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;

                if (string.IsNullOrWhiteSpace(json))
                {
                    Debug.LogWarning($"Remote Config empty json: {key}");
                    return false;
                }

                result = JsonConvert.DeserializeObject<T>(json);

                if (result == null)
                {
                    Debug.LogWarning($"Json parse failed: {key}");
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"GetConfig error ({key}): {e}");
                return false;
            }
        }

        public async UniTask LoadRemoteConfig()
        {
            if (_isInitialized)
                return;
            
            try
            {
                var instance = FirebaseRemoteConfig.DefaultInstance; 
            
                await instance.FetchAsync(TimeSpan.Zero);
                await instance.ActivateAsync();
                _isInitialized = true;
                
                Debug.Log("Remote Config Loaded");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}