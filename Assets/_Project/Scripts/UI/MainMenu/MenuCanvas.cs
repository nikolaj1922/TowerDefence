using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.UI.MainMenu
{
    public class MenuCanvas : MonoBehaviour
    {
        private AnalyticsService _analyticsService;
        private SceneLoader _sceneLoader;
        private ISaveLoad _saveLoad;

        [Header("Buttons")]
        [SerializeField] private Button _goToLevelsMenuButton;

        [Inject]
        public void Construct(SceneLoader sceneLoader, AnalyticsService analyticsService, ISaveLoad saveLoad)
        {
            _sceneLoader = sceneLoader;
            _analyticsService = analyticsService;
            _saveLoad = saveLoad;
        }
        
        private void Awake() =>  _goToLevelsMenuButton.onClick.AddListener(OnStartClick);

        private void OnDestroy() => _goToLevelsMenuButton.onClick.RemoveListener(OnStartClick);
        
        private void OnStartClick()
        {
            _analyticsService.GameStarted(_saveLoad.PlayerProgress.metaCoinsCount);
            _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE).Forget();
        }
    }
}

