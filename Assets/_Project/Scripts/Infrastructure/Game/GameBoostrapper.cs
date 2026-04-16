using System;
using Zenject;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Infrastructure.LoadingCurtain;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private IAssetProvider _assetProvider;
        private LoadingPipelineFactory _loadingPipelineFactory;
        private AssetReferenceGameObject _loadingCurtainView;

        [Inject]
        public void Construct(
            IAssetProvider assetProvider, 
            LoadingPipelineFactory loadingPipelineFactory, 
            [Inject(Id = GameConstants.LOADING_CURTAIN_INJECT_ID)] AssetReferenceGameObject loadingCurtainView)
        {
            _loadingCurtainView = loadingCurtainView;
            _loadingPipelineFactory = loadingPipelineFactory;
            _assetProvider = assetProvider;
        }

        public void Initialize() => StartLoadAssetsAsync().Forget();
        
        private async UniTaskVoid StartLoadAssetsAsync()
        {
            try
            {
                GameObject curtainObject = await _assetProvider.Instantiate(_loadingCurtainView);
                LoadingCurtainView curtain = curtainObject.GetComponent<LoadingCurtainView>();

                await curtain.StartLoadingOperations(_loadingPipelineFactory.GetStartGamePipeline());
                
                _assetProvider.ReleaseInstance(curtainObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}