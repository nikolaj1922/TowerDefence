using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingScene;
using _Project.Scripts.Database.ModalsPrefabDatabase;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModalView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _upgradeButton;

        private SceneLoader _sceneLoader;
        private ModalCreatorService _modalCreatorService;

        [Inject]
        public void Construct(
            SceneLoader sceneLoader,
            ModalCreatorService modalCreatorService
            )
        {
            _sceneLoader = sceneLoader;
            _modalCreatorService = modalCreatorService;
        }
        
        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartClick);
            _upgradeButton.onClick.AddListener(OnOpenUpgradesClick);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartClick);
            _upgradeButton.onClick.RemoveListener(OnOpenUpgradesClick);
        }

        private void OnStartClick()
        {
            _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE).Forget();
            _modalCreatorService.CloseModal();
        }

        private void OnOpenUpgradesClick() =>
            _modalCreatorService.OpenModal(ModalType.Upgrades).Forget();
    }
}

