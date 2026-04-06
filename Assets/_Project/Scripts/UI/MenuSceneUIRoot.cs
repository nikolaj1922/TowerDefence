using Zenject;
using UnityEngine;
using _Project.Scripts.Infrastructure.ModalCreator;
using _Project.Scripts.Database.ModalsPrefabDatabase;

namespace _Project.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class MenuSceneUIRoot : MonoBehaviour
    {
        private ModalCreator _modalCreator;

        [Inject]
        public void Construct(ModalCreator modalCreator) => _modalCreator = modalCreator;

        private void Awake() => _modalCreator.SetUIRoot(GetComponent<RectTransform>());

        private void Start() => _modalCreator.OpenModal(ModalType.Menu);
    }
}