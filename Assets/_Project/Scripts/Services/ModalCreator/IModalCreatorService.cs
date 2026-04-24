using _Project.Scripts.Database.Modals;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Services.ModalCreator
{
    public interface IModalCreatorService
    {
        UniTask<GameObject> OpenModal(ModalType modalType, IInstantiator inPlaceInstantiator = null);
        void CloseModal();
    }
}