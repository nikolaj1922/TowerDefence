using System;
using UnityEngine.UI;
using UnityEngine;

namespace _Project.Scripts.UI.Modals.ShopModal
{
    public class ShopModalView : MonoBehaviour
    {
        public event Action OnBackToMainMenuClicked;
        
        [SerializeField] private Button _backButton;
        [field: SerializeField] public RectTransform GridContainer { get; private set; }
 
        
        private void Awake() => _backButton.onClick.AddListener(OnBackToMainMenuClick);

        private void OnDestroy() => _backButton.onClick.RemoveListener(OnBackToMainMenuClick);
        
        private void OnBackToMainMenuClick() => OnBackToMainMenuClicked?.Invoke();
    }
}