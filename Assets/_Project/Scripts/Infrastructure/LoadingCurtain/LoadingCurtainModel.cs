using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainModel
    {
        public event Action<string> OnNewLoadingOperationStart;
        public event Action<float> OnLoadingOperationProgress;
        
        public async UniTask Run(Queue<ILoadingOperation> loadingOperations)
        {
            int operationCount = loadingOperations.Count;
            float operationCompleted = 0f;
            
            while (loadingOperations.Count > 0)
            {
                ILoadingOperation operation = loadingOperations.Dequeue();
                
                OnNewLoadingOperationStart?.Invoke(operation.Description);
                
                await operation.Load();
                
                operationCompleted++;
                OnLoadingOperationProgress?.Invoke(operationCompleted / operationCount);
            }
        }
    }
}