using _Project.Scripts.Database.ModalsPrefabDatabase;
using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.LoadingScene;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.ModalCreator;

namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModalView : MonoBehaviour
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private TextMeshProUGUI _metaCoinText;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private HorizontalLayoutGroup _metaCoinHorizontalLayoutGroup;

        private ISaveLoad _saveLoad;
        private SceneLoader _sceneLoader;
        private AnalyticsService _analyticsService;
        private ModalCreatorService _modalCreatorService;
        private int _currentWave;

        [Inject]
        public void Construct(
            SceneLoader sceneLoader, 
            AnalyticsService analyticsService,
            ISaveLoad saveLoad,
            ModalCreatorService modalCreatorService
            )
        {
            _modalCreatorService = modalCreatorService;
            _sceneLoader = sceneLoader;
            _saveLoad = saveLoad;
            _analyticsService = analyticsService;
        }

        private void Awake()
        {
            _tryAgainButton.onClick.AddListener(OnTryAgainButtonClick);
            _goToMenuButton.onClick.AddListener(OnGoToMenuButtonClick);
        }

        private void OnDestroy()
        {
            _tryAgainButton.onClick.RemoveListener(OnTryAgainButtonClick);
            _goToMenuButton.onClick.RemoveListener(OnGoToMenuButtonClick);
        }

        public void Initialize(string headerText, int metaCoinAdded,  int currentWave)
        {
            _headerText.text = headerText;
            _metaCoinText.text = $"+{metaCoinAdded}";
            _metaCoinText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                _metaCoinHorizontalLayoutGroup.GetComponent<RectTransform>()
            );
            _currentWave = currentWave;
        }

        private void OnTryAgainButtonClick()
        {
            _analyticsService.SessionRestarted(_currentWave);
            _sceneLoader
                .LoadScene(
                    GameConstants.LEVEL_SCENE,
                    _modalCreatorService.CloseModal)
                .Forget();
           
        }

        private void OnGoToMenuButtonClick()
        {
            _analyticsService.ReturnedToMenu(
                _currentWave,
                _saveLoad.PlayerProgress.metaCoinsCount);
            _sceneLoader
                .LoadScene(
                    GameConstants.MENU_SCENE,
                    () => _modalCreatorService.OpenModal(ModalType.Menu))
                .Forget();
        }
    }
}

