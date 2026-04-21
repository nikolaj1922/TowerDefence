using System;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.UI.MetaCounter;
using _Project.Scripts.UI.Modals.UpgradeModal.UpgradeButton;

namespace _Project.Scripts.UI.Modals.UpgradeModal
{
    public class UpgradeModalView : MonoBehaviour
    {
        public event Action OnBackToMainMenuClicked;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private UpgradeButtonView _upgradeButtonPrefab;
        
        [field: SerializeField] public MetaCounterView MetaCounterView { get; private set; }
        [field: SerializeField] public RectTransform GridContainer { get; private set; }
        
        private void Awake() => _backButton.onClick.AddListener(OnBackToMainMenuClick);
        
        private void OnDestroy() => _backButton.onClick.RemoveListener(OnBackToMainMenuClick);

        private void OnBackToMainMenuClick() => OnBackToMainMenuClicked?.Invoke();
    }
}