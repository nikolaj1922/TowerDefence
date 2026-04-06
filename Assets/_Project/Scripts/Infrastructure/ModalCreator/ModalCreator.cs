using Zenject;
using UnityEngine;
using _Project.Scripts.Database.ModalsPrefabDatabase;

namespace _Project.Scripts.Infrastructure.ModalCreator
{
    public class ModalCreator
    {
        [Inject] private IInstantiator _instantiator;
        
        private GameObject _currentOpenedModal;
        private readonly ModalsPrefabDatabase _modalPrefabDatabase;
        private RectTransform _uiRoot;

        public ModalCreator(ModalsPrefabDatabase modalPrefabDatabase)
        {
            _modalPrefabDatabase =  modalPrefabDatabase;
        }

        public void SetUIRoot(RectTransform uiRoot) => _uiRoot = uiRoot;

        public void OpenModal(ModalType modalType)
        {
            if (_currentOpenedModal != null)
                CloseModal();

            _currentOpenedModal = _instantiator.InstantiatePrefab(_modalPrefabDatabase.Get(modalType), _uiRoot);
        }

        public void CloseModal() => Object.Destroy(_currentOpenedModal);
    }
}