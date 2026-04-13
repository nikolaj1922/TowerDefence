using System;
using _Project.Scripts.UI.CoinCounter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerItemButton : MonoBehaviour
    {
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private CoinCounterModel _coinCounterModel;
        private int _price;

        private void OnDestroy()
        {
            _createTowerButton.onClick.RemoveAllListeners();
            _coinCounterModel.OnCoinChanged -= UpdateButtonStatus;
        }
        
        public void Initialize(int price, Sprite preview, Action onClick, CoinCounterModel coinCounterModel)
        {
            _coinCounterModel = coinCounterModel;
            _price = price;
            
            _preview.sprite = preview;
            _priceText.text = _price.ToString();
            
            _createTowerButton.onClick.AddListener(() => onClick?.Invoke());
            
            _coinCounterModel.OnCoinChanged += UpdateButtonStatus;
            _coinCounterModel.UpdateSubscribers();
            
            UpdateGroupLayout();
        }

        private void UpdateGroupLayout()
        {
            _priceText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
        }
        
        private void UpdateButtonStatus(int currentCoin)
        {
            bool isCoinEnough = currentCoin >= _price;
            _priceText.color = isCoinEnough ? Color.white : Color.red;
        }
    }
}