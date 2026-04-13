using _Project.Scripts.Database.ModalsPrefabDatabase;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.ModalCreator
{
    public class ModalCreatorService
    {
        private readonly IInstantiator _instantiator;
        private readonly ModalsPrefabDatabase _modalPrefabDatabase;

        private RectTransform _uiRoot;
        private GameObject _currentOpenedModal;

        public ModalCreatorService(ModalsPrefabDatabase modalPrefabDatabase, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _modalPrefabDatabase =  modalPrefabDatabase;
        }

        public void SetUIRoot(RectTransform uiRoot) => _uiRoot = uiRoot;

        public GameObject OpenModal(ModalType modalType)
        {
            if (_currentOpenedModal != null)
                CloseModal();

            _currentOpenedModal = _instantiator.InstantiatePrefab(_modalPrefabDatabase.Get(modalType), _uiRoot);
            return _currentOpenedModal;
        }

        public void CloseModal() => Object.Destroy(_currentOpenedModal);
    }
}