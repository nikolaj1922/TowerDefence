using _Project.Scripts.Database.Modals;
using _Project.Scripts.Services.AssetProvider;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.ModalCreator
{
    public class ModalCreatorService : IModalCreatorService
    {
        private readonly IInstantiator _instantiator;
        private readonly ModalsPrefabsDatabase _modalPrefabsDatabase;
        private readonly IAssetProvider _assetProvider;

        private GameObject _currentOpenedModal;

        public ModalCreatorService(ModalsPrefabsDatabase modalPrefabsDatabase, IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _modalPrefabsDatabase =  modalPrefabsDatabase;
        }


        public async UniTask<GameObject> OpenModal(ModalType modalType)
        {
            if (_currentOpenedModal != null)
                CloseModal();

            GameObject modalObject = await _assetProvider.Load<GameObject>(_modalPrefabsDatabase.Get(modalType));
            _currentOpenedModal = _instantiator.InstantiatePrefab(modalObject);
            return _currentOpenedModal;
        }

        public void CloseModal() => Object.Destroy(_currentOpenedModal);
    }
}