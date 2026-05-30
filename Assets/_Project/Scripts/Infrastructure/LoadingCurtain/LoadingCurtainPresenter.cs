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
        private readonly IAssetProviderService _assetProviderService;
        
        public LoadingCurtainPresenter(
            [Inject(Id = GameConstants.LOADING_CURTAIN_ASSET_INJECT_ID)] AssetReferenceGameObject viewReference, 
            LoadingCurtainModel model,
            IAssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
            _model = model;
            _viewReference = viewReference;
        }

        public async UniTask StartLoadingOperations(Queue<ILoadingOperation> loadingOperations) =>
            await Run(loadingOperations);
        
        private async UniTask Run(Queue<ILoadingOperation> loadingOperations)
        {
            Debug.Log(Addressables.RuntimePath);
            
            foreach (var locator in Addressables.ResourceLocators)
            {
                Debug.Log(locator.LocatorId);
            }
            
            var locations = Addressables.LoadResourceLocationsAsync(_viewReference);
            await locations;

            foreach (var loc in locations.Result)
            {
                Debug.Log($"Primary: {loc.InternalId}");

                if (loc.Dependencies != null)
                {
                    foreach (var dep in loc.Dependencies)
                    {
                        Debug.Log($"Dependency: {dep.InternalId}");
                    }
                }
            }
            GameObject viewObject = await _assetProviderService.Instantiate(_viewReference);
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
            _assetProviderService.ReleaseInstance(viewObject);
        }
    }
}