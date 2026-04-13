using Zenject;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using _Project.Scripts.Database;
using _Project.Scripts.UI.MetaCounter;
using _Project.Scripts.Services.SaveLoad;
using _Project.Scripts.Infrastructure.ModalCreator;
using _Project.Scripts.Database.ModalsPrefabDatabase;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.UI.Modals.UpgradeModal
{
    public class UpgradeModal : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _gridContainer;
        [SerializeField] private MetaCounterPanel _metaCounterPanel;
        [SerializeField] private UpgradeItemView _upgradeItemPrefab;

        private ISaveLoad _saveLoad;
        private ModalCreator _modalCreator;
        private TowerUpgradeService _towerUpgradeService;
        private UpgradesDatabase _upgradeDatabase;

        private List<UpgradeItemPresenter> _upgradeItemPresenters;

        [Inject]
        public void Construct(
            UpgradesDatabase upgradesDatabase, 
            ISaveLoad saveLoad, 
            ModalCreator modalCreator, 
            TowerUpgradeService towerUpgradeService)
        {
            _towerUpgradeService = towerUpgradeService;
            _modalCreator = modalCreator;
            _upgradeDatabase = upgradesDatabase;
            _saveLoad = saveLoad;
        }

        private void Awake() => _backButton.onClick.AddListener(BackToMainMenu);

        private void Start() => CreateUpgradeList();
        
        private void OnDestroy()
        {
            foreach (var upgradeItemPresenter in _upgradeItemPresenters)
                upgradeItemPresenter.Dispose();
            
            _backButton.onClick.RemoveListener(BackToMainMenu);
        }

        private void BackToMainMenu() => _modalCreator.OpenModal(ModalType.Menu);
        
        private void CreateUpgradeList()
        {
            _upgradeItemPresenters = new List<UpgradeItemPresenter>();
            
            foreach (var upgrade in _upgradeDatabase.upgrades)
            {
                var view = Instantiate(_upgradeItemPrefab, _gridContainer);

                _upgradeItemPresenters.Add(new UpgradeItemPresenter(
                    _saveLoad,
                    view,
                    upgrade,
                    _towerUpgradeService
                ));
                
                view.OnBuyClicked += _metaCounterPanel.UpdateView;
            }
        }
    }
}