using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.Firebase
{
    public class FirebaseInitializer: IInitializable
    {
        public void Initialize() => InitFirebase().Forget();

        private async UniTask InitFirebase()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependencyStatusReceived).AsUniTask();
        }

        private void OnDependencyStatusReceived(Task<DependencyStatus> task)
        {
            try
            {
                if(!task.IsCompletedSuccessfully)
                    throw new Exception($"Failed to get dependency status: {task.Result}");
                
                var status = task.Result;
                
                if (status != DependencyStatus.Available)
                    throw new Exception($"Failed to get dependency status: {status}");
                
                Debug.Log("Firebase initialized successfully");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}