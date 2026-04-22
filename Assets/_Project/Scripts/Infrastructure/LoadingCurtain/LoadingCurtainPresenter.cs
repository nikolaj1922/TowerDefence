using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;
using _Project.Scripts.Services.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainPresenter
    {
        private readonly AssetReferenceGameObject _viewReference;
        private readonly LoadingCurtainModel _model;
        private readonly IAssetProvider _assetProvider;
        
        public LoadingCurtainPresenter(
            [Inject(Id = GameConstants.LOADING_CURTAIN_ASSET_INJECT_ID)] AssetReferenceGameObject viewReference, 
            LoadingCurtainModel model,
            IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _model = model;
            _viewReference = viewReference;
        }

        public async UniTask StartLoadingOperations(Queue<ILoadingOperation> loadingOperations) =>
            await Run(loadingOperations);
        
        private async UniTask Run(Queue<ILoadingOperation> loadingOperations)
        {
            GameObject viewObject = await _assetProvider.Instantiate(_viewReference);
            LoadingCurtainView curtain = viewObject.GetComponent<LoadingCurtainView>();
            
            curtain.Reset();
            curtain.gameObject.SetActive(true);
            
            _model.SetOperationCount(loadingOperations.Count);
            
            while (loadingOperations.Count > 0)
            {
                ILoadingOperation operation = loadingOperations.Dequeue();
                
                curtain.SetDescription(operation.Description);
                
              
                curtain.DrawProgress(_model);
                
                await operation.Load();
                
                _model.CompleteOperation();
            }

            if (viewObject == null)
                return;
            
            curtain.gameObject.SetActive(false);
            _assetProvider.ReleaseInstance(viewObject);
        }
    }
}