using System;
using _Project.Scripts.Logic.Wave;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Services.Upgrade;
using _Project.Scripts.Towers;
using _Project.Scripts.UI.CoinCounter;
using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerItemButton : MonoBehaviour
    {
        public Action<int> onCreateTower;
        
        [SerializeField] private Image _preview;
        [SerializeField] private Button _createTowerButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        private WaveManager _waveManager;
        private AnalyticsService _analyticsService;
        private CoinCounterModel _coinCounterModel;
        private TowerService _towerService;
        private UpgradeService _upgradeService;
        private CreateTowerView _towerView;
        private TowerType _towerType;
        private int _price;

        [Inject]
        public void Construct(
            AnalyticsService analyticsService, 
            CoinCounterModel coinCounterModel,
            TowerService towerService,
            UpgradeService upgradeService,
            WaveManager waveManager
            )
        {
            _waveManager = waveManager;
            _analyticsService = analyticsService;
            _coinCounterModel = coinCounterModel;
            _towerService = towerService;
            _upgradeService = upgradeService;
        }

        private void OnDestroy()
        {
            _createTowerButton.onClick.RemoveListener(CreateTower);
            _coinCounterModel.OnCoinChanged -= UpdateButtonStatus;
        }
        
        public void Initialize(
            int price, 
            Sprite preview, 
            TowerType towerType, 
            CreateTowerView towerView
            )
        {
            _price = price;
            _towerView = towerView;
            _towerType = towerType;
            _preview.sprite = preview;
            _priceText.text = _price.ToString();
            
            _createTowerButton.onClick.AddListener(CreateTower);
            
            _coinCounterModel.OnCoinChanged += UpdateButtonStatus;
            _coinCounterModel.UpdateSubscribers();
            
            UpdateGroupLayout();
        }
        
        private void CreateTower()
        {
            if (_coinCounterModel.Coins < _price)
            {
                _analyticsService.BuildRejected("not_enough_coins", _waveManager.CurrentWave);
                return;
            }   
            
            _towerService.CreateAndPurchase(
                _towerType, 
                _towerView.CreateTowerPosition, 
                _price,
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.TOWER_DAMAGE_ID),
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.TOWER_ATTACK_SPEED_ID)
            );
            _towerView.HidePanel();
            
            onCreateTower?.Invoke(_price);
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