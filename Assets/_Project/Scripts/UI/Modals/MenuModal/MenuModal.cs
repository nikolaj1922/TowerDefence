using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Infrastructure.ModalCreator;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModal : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _upgradeButton;

        private SceneLoader _sceneLoader;
        private ModalCreator _modalCreator;

        [Inject]
        public void Construct(
            SceneLoader sceneLoader,
            ModalCreator modalCreator
            )
        {
            _sceneLoader = sceneLoader;
            _modalCreator = modalCreator;
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
        
        private void OnStartClick() => _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE).Forget();

        private void OnOpenUpgradesClick() =>
            _modalCreator.OpenModal(ModalType.Upgrades);
    }
}

