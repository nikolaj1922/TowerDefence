using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModal : MonoBehaviour
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private TextMeshProUGUI _metaCoinText;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private HorizontalLayoutGroup _metaCoinHorizontalLayoutGroup;

        private ISaveLoad _saveLoad;
        private SceneLoader _sceneLoader;
        private WaveManager _waveManager;
        private AnalyticsService _analyticsService;

        [Inject]
        public void Construct(
            SceneLoader sceneLoader, 
            WaveManager waveManager, 
            AnalyticsService analyticsService,
            ISaveLoad saveLoad
            )
        {
            _waveManager = waveManager;
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
        
        public void SetHeaderText(string headerText) => _headerText.text = headerText;

        public void SetMetaCoinText(int metaCoinAdded)
        {
            _metaCoinText.text = $"+{metaCoinAdded}";
            _metaCoinText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                _metaCoinHorizontalLayoutGroup.GetComponent<RectTransform>()
            );
        }

        private void OnTryAgainButtonClick()
        {
            _analyticsService.SessionRestarted(_waveManager.CurrentWave);
            _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE).Forget();
        }

        private void OnGoToMenuButtonClick()
        {
            _analyticsService.ReturnedToMenu(
                _waveManager.CurrentWave,
                _saveLoad.PlayerProgress.metaCoinsCount);
            _sceneLoader.LoadScene(GameConstants.MENU_SCENE).Forget();
        }
    }
}

