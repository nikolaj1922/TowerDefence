using Zenject;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using _Project.Scripts.Database;
using _Project.Scripts.UI.MetaCounter;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Services.ModalCreator;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.UI.Modals.UpgradeModal.UpgradeItem;

namespace _Project.Scripts.UI.Modals.UpgradeModal
{
    public class UpgradeModalView : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _gridContainer;
        [SerializeField] private MetaCounterPanel _metaCounterPanel;
        [SerializeField] private UpgradeItemView _upgradeItemPrefab;

        private ISaveLoad _saveLoad;
        private TowerUpgradeService _towerUpgradeService;
        private ModalCreatorService _modalCreatorService;
        private UpgradesDatabase _upgradeDatabase;

        private List<UpgradeItemPresenter> _upgradeItemPresenters;

        [Inject]
        public void Construct(
            UpgradesDatabase upgradesDatabase, 
            ISaveLoad saveLoad, 
            ModalCreatorService modalCreatorService, 
            TowerUpgradeService towerUpgradeService)
        {
            _towerUpgradeService = towerUpgradeService;
            _modalCreatorService = modalCreatorService;
            _upgradeDatabase = upgradesDatabase;
            _saveLoad = saveLoad;
        }

        private void Awake() => _backButton.onClick.AddListener(BackToMainMenu);

        private void Start() => CreateUpgradeList();
        
        private void OnDestroy()
        {
            foreach (var upgradeItemPresenter in _upgradeItemPresenters)
            {
                upgradeItemPresenter.Dispose();
                upgradeItemPresenter.View.OnBuyClicked -= RefreshListAfterBuy;
            }
              
            
            _backButton.onClick.RemoveListener(BackToMainMenu);
        }

        private void BackToMainMenu() => _modalCreatorService.OpenModal(ModalType.Menu);
        
        private void CreateUpgradeList()
        {
            _upgradeItemPresenters = new List<UpgradeItemPresenter>();
            
            foreach (var upgrade in _upgradeDatabase.upgrades)
            {
                var upgradePrefabView = Instantiate(_upgradeItemPrefab, _gridContainer);

                UpgradeItemPresenter upgradeItemPresenter = new UpgradeItemPresenter(
                    _saveLoad,
                    upgradePrefabView,
                    upgrade,
                    _towerUpgradeService
                );
                upgradeItemPresenter.View.OnBuyClicked += RefreshListAfterBuy;
                
                _upgradeItemPresenters.Add(upgradeItemPresenter);

            }
        }

        private void RefreshListAfterBuy()
        {
            _metaCounterPanel.UpdateView();

            foreach (UpgradeItemPresenter presenter in _upgradeItemPresenters)
                presenter.Refresh();
        }
    }
}