using Zenject;
using UnityEngine;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Services.ModalCreator;

namespace _Project.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class MenuSceneUIRoot : MonoBehaviour
    {
        private ModalCreatorService _modalCreatorService;

        [Inject]
        public void Construct(ModalCreatorService modalCreatorService) => _modalCreatorService = modalCreatorService;

        private void Awake() => _modalCreatorService.SetUIRoot(GetComponent<RectTransform>());

        private void Start() => _modalCreatorService.OpenModal(ModalType.Menu);
    }
}