using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainView : MonoBehaviour
    {
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TextMeshProUGUI _description;

        
        public async UniTask StartLoadingOperations(Queue<ILoadingOperation> loadingOperations)
        {
            gameObject.SetActive(true);
            
            ResetLoadingProgress();
            await Run(loadingOperations);
            
            gameObject.SetActive(false);
        }
        
        private async UniTask Run(Queue<ILoadingOperation> loadingOperations)
        {
            int operationCount = loadingOperations.Count;
            float operationCompleted = 0f;
            
            while (loadingOperations.Count > 0)
            {
                ILoadingOperation operation = loadingOperations.Dequeue();
                
                SetDescription(operation.Description);
                
                await operation.Load();
                
                operationCompleted++;
                SetLoadingProgress(operationCompleted / operationCount);
            }
        }
        
        private void SetDescription(string description) => _description.text = description;

        private void SetLoadingProgress(float progress) => _loadingSlider.value = progress;
        
        private void ResetLoadingProgress() => _loadingSlider.value = 0;
    }
}