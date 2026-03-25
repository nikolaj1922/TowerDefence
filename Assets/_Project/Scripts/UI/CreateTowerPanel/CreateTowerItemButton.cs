using _Project.Scripts.Logic.Coins;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace _Project.Scripts.UI.CreateTowerPanel
{
    public class CreateTowerItemButton : MonoBehaviour
    {
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;

        private UnityAction _onClick;
        private int _price;
        
        private void OnDestroy() =>  _createTowerButton.onClick.RemoveAllListeners();
        
        public void Initialize(int price, Sprite preview, UnityAction onClick, CoinCounterModel coinCounterModel)
        {
            _price = price;
            
            _preview.sprite = preview;
            _priceText.text = _price.ToString();
            _createTowerButton.onClick.AddListener(onClick);

            coinCounterModel.OnCoinChanged += UpdateButton;
        }

        private void UpdateButton(int currentCoin)
        {
            bool isCoinEnough = currentCoin >= _price;
            
            _createTowerButton.interactable = isCoinEnough;
            _priceText.color = isCoinEnough ? Color.white : Color.red;
        }
    }
}