using _Project.Scripts.Database.Modals;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Services.ModalCreator
{
    public interface IModalCreatorService
    {
        UniTask<GameObject> OpenModal(ModalType modalType);
        void CloseModal();
    }
}