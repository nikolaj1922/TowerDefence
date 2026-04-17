using _Project.Scripts.Database.ModalsPrefabDatabase;
using UnityEngine;

namespace _Project.Scripts.Services.ModalCreator
{
    public interface IModalCreatorService
    {
        GameObject OpenModal(ModalType modalType);
        void CloseModal();
    }
}