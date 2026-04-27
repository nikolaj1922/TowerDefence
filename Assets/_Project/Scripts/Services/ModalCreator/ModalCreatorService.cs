using _Project.Scripts.Database.Modals;
using _Project.Scripts.Services.AssetProvider;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Services.ModalCreator
{
    public class ModalCreatorService : IModalCreatorService
    {
        private readonly IInstantiator _instantiator;
        private readonly ModalsDatabase _modalDatabase;
        private readonly IAssetProviderService _assetProviderService;

        private GameObject _currentOpenedModal;
        private AssetReferenceGameObject _currentAssetReferenceGameObject;

        public ModalCreatorService(ModalsDatabase modalDatabase, IAssetProviderService assetProviderService, IInstantiator instantiator)
        {
            _assetProviderService = assetProviderService;
            _instantiator = instantiator;
            _modalDatabase =  modalDatabase;
        }


        public async UniTask<GameObject> OpenModal(ModalType modalType, IInstantiator inPlaceInstantiator)
        {
            if (_currentOpenedModal != null)
                CloseModal();
            
            IInstantiator instantiatorInstance = inPlaceInstantiator ?? _instantiator;
            
            _currentAssetReferenceGameObject = _modalDatabase.Get(modalType);
            GameObject modalObject = await _assetProviderService.Load<GameObject>(_currentAssetReferenceGameObject);
            _currentOpenedModal = instantiatorInstance.InstantiatePrefab(modalObject);
            return _currentOpenedModal;
        }

        public void CloseModal()
        {
            Object.Destroy(_currentOpenedModal);
            _assetProviderService.Release(_currentAssetReferenceGameObject);

            _currentOpenedModal = null;
            _currentAssetReferenceGameObject = null;
        }
    }
}