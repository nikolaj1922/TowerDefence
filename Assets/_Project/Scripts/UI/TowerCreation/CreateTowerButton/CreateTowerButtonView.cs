using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation.CreateTowerButton
{
    public class CreateTowerButtonView : MonoBehaviour
    {
        public event Action<int> OnCreateTower;
        
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private int _price;
        
        private void Awake() => _createTowerButton.onClick.AddListener(OnCreateTowerClick);

        private void OnDestroy() => _createTowerButton.onClick.RemoveListener(OnCreateTowerClick);
        
        public void Initialize(Sprite preview, int price)
        {
            _price = price;
            _preview.sprite = preview;
            _priceText.text = price.ToString();
        }
        
        public void Draw(int currentCoin, int price)
        {
            bool isCoinEnough = currentCoin >= price;
            _priceText.color = isCoinEnough ? Color.white : Color.red;
            
            UpdateGroupLayout();
        }
        
        private void OnCreateTowerClick() => OnCreateTower?.Invoke(_price);

        private void UpdateGroupLayout()
        {
            _priceText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
        }
    }
}