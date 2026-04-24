using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Modals.ContinueForAdsModal
{
    public class ContinueForAdsModalView : MonoBehaviour
    {
        public event Action OnEndGameClick; 
        public event Action OnWatchAdsClick; 
        
        [SerializeField] private Button _endGameButton;
        [SerializeField] private Button _watchAdsButton;

        private void Start()
        {
            _endGameButton.onClick.AddListener(OnEndGameButtonClick);
            _watchAdsButton.onClick.AddListener(OnWatchAdsButtonClick);
        }

        private void OnDestroy()
        {
            _endGameButton.onClick.RemoveListener(OnEndGameButtonClick);
            _watchAdsButton.onClick.RemoveListener(OnWatchAdsButtonClick);
        }

        private void OnEndGameButtonClick() => OnEndGameClick?.Invoke();
        private  void OnWatchAdsButtonClick() => OnWatchAdsClick?.Invoke();
    }
}