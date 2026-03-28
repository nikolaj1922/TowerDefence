using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using _Project.Scripts.Logic.Coins;

namespace _Project.Scripts.UI.CreateTowerPanel
{
    public class CreateTowerItemButton : MonoBehaviour
    {
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private int _price;
        
        private void OnDestroy() =>  _createTowerButton.onClick.RemoveAllListeners();
        
        public void Initialize(int price, Sprite preview, UnityAction onClick, CoinCounterModel coinCounterModel)
        {
            _price = price;
            
            _preview.sprite = preview;
            _priceText.text = _price.ToString();
            _createTowerButton.onClick.AddListener(onClick);
            
            _priceText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());

            coinCounterModel.OnCoinChanged += UpdateButtonStatus;
            coinCounterModel.UpdateSubscribers();
        }
        
        private void UpdateButtonStatus(int currentCoin)
        {
            bool isCoinEnough = currentCoin >= _price;
            _createTowerButton.interactable = isCoinEnough;
            _priceText.color = isCoinEnough ? Color.white : Color.red;
        }
    }
}