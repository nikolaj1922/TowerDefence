using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModalView : MonoBehaviour
    {
        public event Action OnStartClicked;
        public event Action OnOpenUpgradesClicked;
        
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _upgradeButton;
        
        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        }
        
        private void OnStartButtonClicked() => OnStartClicked?.Invoke();
        private void OnUpgradeButtonClicked() => OnOpenUpgradesClicked?.Invoke();
    }
}

