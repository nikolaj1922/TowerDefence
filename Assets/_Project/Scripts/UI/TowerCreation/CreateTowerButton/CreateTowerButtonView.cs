using System;
using _Project.Scripts.Services.AssetProvider;
using _Project.Scripts.Towers;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.TowerCreation.CreateTowerButton
{
    public class CreateTowerButtonView : MonoBehaviour
    {
        public event Action<int, TowerType> OnCreateTower;
        
        private IAssetProviderService _assetProviderService;
        
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private int _price;
        private string _iconAddress;
        private TowerType _towerType;

        [Inject]
        public void Construct(IAssetProviderService assetProviderService) =>
            _assetProviderService = assetProviderService;

        private void Awake() => _createTowerButton.onClick.AddListener(OnCreateTowerClick);

        private void OnDestroy()
        {
            _createTowerButton.onClick.RemoveListener(OnCreateTowerClick);
            _assetProviderService.ReleaseByAddress(_iconAddress);
        }
        
        public void Initialize(string iconAddress, int price, TowerType towerType)
        {
            _towerType = towerType;
            _price = price;
            _priceText.text = price.ToString();
            _iconAddress = iconAddress;
            LoadPreview().Forget();
        }
        
        public void Draw(int currentCoin)
        {
            bool isCoinEnough = currentCoin >= _price;
            _priceText.color = isCoinEnough ? Color.white : Color.red;
            
            UpdateGroupLayout();
        }

        private async UniTask LoadPreview()
        {
            Sprite previewSprite = await _assetProviderService.LoadByAddress<Sprite>(_iconAddress);
            _preview.sprite = previewSprite;
        }
        
        private void OnCreateTowerClick() => OnCreateTower?.Invoke(_price, _towerType);

        private void UpdateGroupLayout()
        {
            _priceText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
        }
    }
}