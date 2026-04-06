using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.SceneLoader;

namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModal : MonoBehaviour
    {
        private SceneLoader _sceneLoader;

        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private TextMeshProUGUI _metaCoinText;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private HorizontalLayoutGroup _metaCoinHorizontalLayoutGroup;

        [Inject]
        public void Construct(SceneLoader sceneLoader) => _sceneLoader = sceneLoader;

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

        private void OnTryAgainButtonClick() => _sceneLoader.LoadScene(GameConstants.LEVEL_SCENE).Forget();
    
        private void OnGoToMenuButtonClick() => _sceneLoader.LoadScene(GameConstants.MENU_SCENE).Forget();
    }
}

