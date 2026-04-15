using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Modals.UpgradeModal.UpgradeItem
{
    public class UpgradeItemView : MonoBehaviour
    {
        public event Action OnBuyClicked;
        
        [SerializeField] private Image _icon;
        [SerializeField] private Image _metaCoinIcon;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        
        private void Awake() => _buyButton.onClick.AddListener(OnBuyButtonClick);

        private void OnDestroy() => _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        
        private void OnBuyButtonClick() => OnBuyClicked?.Invoke();
        
        public void SetPrice(string price, Color color)
        {
            _price.text = price;
            _price.color = color;
        }
        
        public void SetTitle(string text) => _title.text = text;
        
        public void SetIcon(Sprite icon) => _icon.sprite = icon;
        
        public void SetDescription(string description) => _description.text = description;
        
        public void SetInteractable(bool interactable) => _buyButton.interactable = interactable;
        
        public void SetMetaIconActive(bool active) => _metaCoinIcon.gameObject.SetActive(active);
    }
}