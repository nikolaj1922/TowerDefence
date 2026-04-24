using System;
using _Project.Scripts.Towers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation.CreateTowerButton
{
    public class CreateTowerButtonView : MonoBehaviour
    {
        public event Action<int, TowerType> OnCreateTower;
        
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private int _price;
        private TowerType _towerType;
        
        private void Awake() => _createTowerButton.onClick.AddListener(OnCreateTowerClick);

        private void OnDestroy() => _createTowerButton.onClick.RemoveListener(OnCreateTowerClick);
        
        public void Initialize(Sprite preview, int price, TowerType towerType)
        {
            _towerType = towerType;
            _price = price;
            _preview.sprite = preview;
            _priceText.text = price.ToString();
        }
        
        public void Draw(int currentCoin)
        {
            bool isCoinEnough = currentCoin >= _price;
            _priceText.color = isCoinEnough ? Color.white : Color.red;
            
            UpdateGroupLayout();
        }
        
        private void OnCreateTowerClick() => OnCreateTower?.Invoke(_price, _towerType);

        private void UpdateGroupLayout()
        {
            _priceText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
        }
    }
}