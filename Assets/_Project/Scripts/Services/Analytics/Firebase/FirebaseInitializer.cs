using System;
using Zenject;
using Firebase;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;

namespace _Project.Scripts.Services.Analytics.Firebase
{
    public class FirebaseInitializer: IInitializable
    {
        public void Initialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(OnDependencyStatusReceived);
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