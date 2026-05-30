using System;
using _Project.Scripts.UI.MetaCounter;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Modals.MenuModal
{
    public class MenuModalView : MonoBehaviour
    {
        public event Action OnStartClicked;
        public event Action OnShopClicked;
        public event Action OnOpenUpgradesClicked;
        public event Action OnQuitClicked;

        [SerializeField] private MetaCounterView _metaCounterView;
        
        [Header("Buttons")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _quitButton;
        
        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            _shopButton.onClick.AddListener(OnShopButtonClicked);
            _quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
            _shopButton.onClick.RemoveListener(OnShopButtonClicked);
            _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        public void UpdateMetaCounter(string metaCounterText) => _metaCounterView.UpdateView(metaCounterText);
        
        private void OnStartButtonClicked() => OnStartClicked?.Invoke();
        private void OnShopButtonClicked() => OnShopClicked?.Invoke();
        private void OnUpgradeButtonClicked() => OnOpenUpgradesClicked?.Invoke();
        private void OnQuitButtonClicked() => OnQuitClicked?.Invoke();
    }
}

