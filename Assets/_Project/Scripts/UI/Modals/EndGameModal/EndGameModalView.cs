using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Modals.EndGameModal
{
    public class EndGameModalView : MonoBehaviour
    {
        public event Action OnTryAgainButtonClicked;
        public event Action OnGoToMenuButtonClicked;
        
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _goToMenuButton;
        [SerializeField] private TextMeshProUGUI _metaCoinText;
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private HorizontalLayoutGroup _metaCoinHorizontalLayoutGroup;

        public int CurrentWave { get; private set; }

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

        private void OnTryAgainButtonClick() => OnTryAgainButtonClicked?.Invoke();
        
        private void OnGoToMenuButtonClick() => OnGoToMenuButtonClicked?.Invoke();
        
        public void SetCurrentWave(int wave) => CurrentWave = wave;

        public void Draw(string headerText, int metaCoinAdded)
        {
            _headerText.text = headerText;
            _metaCoinText.text = $"+{metaCoinAdded}";
            _metaCoinText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                _metaCoinHorizontalLayoutGroup.GetComponent<RectTransform>()
            );
        }
    }
}

