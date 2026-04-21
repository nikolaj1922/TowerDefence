using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Services.ModalCreator;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainFactory : ILoadingCurtainFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly AssetReferenceGameObject _loadingCurtainView;
        private readonly IModalCreatorService  _modalCreatorService;

        public LoadingCurtainFactory(
            IAssetProvider assetProvider, 
            AssetReferenceGameObject loadingCurtainView,
            IModalCreatorService modalCreatorService)
        {
            _modalCreatorService = modalCreatorService;
            _assetProvider = assetProvider;
            _loadingCurtainView = loadingCurtainView;
        }
        
        public async UniTaskVoid Create(Queue<ILoadingOperation> loadingOperations)
        {
            try
            {
                GameObject curtainObject = await _assetProvider.Instantiate(_loadingCurtainView);
                LoadingCurtainView curtain = curtainObject.GetComponent<LoadingCurtainView>();
                
                _modalCreatorService.CloseModal();

                await curtain.StartLoadingOperations(loadingOperations);
                
                _assetProvider.ReleaseInstance(curtainObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}