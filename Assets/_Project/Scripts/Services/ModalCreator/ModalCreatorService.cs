using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Services.AssetProvider;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.ModalCreator
{
    public class ModalCreatorService
    {
        private readonly IInstantiator _instantiator;
        private readonly ModalsPrefabDatabase _modalPrefabDatabase;
        private readonly IAssetProvider _assetProvider;

        private GameObject _currentOpenedModal;

        public ModalCreatorService(ModalsPrefabDatabase modalPrefabDatabase, IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _modalPrefabDatabase =  modalPrefabDatabase;
        }


        public async UniTask<GameObject> OpenModal(ModalType modalType)
        {
            if (_currentOpenedModal != null)
                CloseModal();

            GameObject modalObject = await _assetProvider.Load<GameObject>(_modalPrefabDatabase.Get(modalType));
            _currentOpenedModal = _instantiator.InstantiatePrefab(modalObject);
            return _currentOpenedModal;
        }

        public void CloseModal() => Object.Destroy(_currentOpenedModal);
    }
}