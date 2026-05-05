using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.UI.Modals.ShopModal
{
    [RequireComponent(typeof (IAPListener))]
    public class ShopModalView : MonoBehaviour
    {
        public event Action OnBackToMainMenuClicked;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _noAdsButton;
        private IAPListener _iapListener;

        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackToMainMenuClick);
        }

        public IAPListener GetAipListener()
        {
            if(_iapListener == null)
                _iapListener = GetComponent<IAPListener>();

            return _iapListener;
        }
        
        private void OnDestroy() => _backButton.onClick.RemoveListener(OnBackToMainMenuClick);
        
        public void HideNoAdsButton() => _noAdsButton.SetActive(false);

        private void OnBackToMainMenuClick() => OnBackToMainMenuClicked?.Invoke();
    }
}