using System;
using System.Linq;
using UnityEngine;
using _Project.Scripts.Configs.Upgrades;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.UI.Modals.UpgradeModal.UpgradeItem
{
    public class UpgradeItemPresenter :IDisposable
    {
        private readonly ISaveLoad _saveLoad;
        private readonly UpgradeItemView _view;
        private readonly PlayerProgress _progress;
        private readonly UpgradeBaseConfig _upgradeConfig;

        private UpgradeLevel _levelToBuy;

        public UpgradeItemPresenter(ISaveLoad saveLoad, UpgradeItemView view, UpgradeBaseConfig upgradeConfig)
        {
            _view = view;
            _saveLoad = saveLoad;
            _progress = saveLoad.PlayerProgress;
            _upgradeConfig = upgradeConfig;

            Init();
        }

        private void Init()
        {
            Refresh();
            
            _view.OnBuyClicked += OnBuyClicked;
            _view.SetTitle(_upgradeConfig.title);
            _view.SetIcon(_upgradeConfig.previewIcon);
        }
        
        private void OnBuyClicked()
        {
            if (_levelToBuy == null)
                return;
            
            if (_progress.metaCoinsCount < _levelToBuy.price)
                return;
            
            _progress.SetUpgradeLevel(_upgradeConfig.id, _levelToBuy.level);
            _progress.metaCoinsCount -= _levelToBuy.price;
            _upgradeConfig.OnBuy(_progress, _levelToBuy.multiplier);
            
            _saveLoad.SaveProgress();
            Refresh();
        }

        private void Refresh()
        {
            int level = _progress.GetUpgradeLevel(_upgradeConfig.id);
            _levelToBuy = _upgradeConfig.levels.FirstOrDefault(x => x.level == level + 1);

            if (_levelToBuy == null)
            {
                _view.SetPrice("MAX", Color.white);
                _view.SetMetaIconActive(false);
                _view.SetInteractable(false);
                return;
            }
            
            bool canBuy = _progress.metaCoinsCount >= _levelToBuy.price;

            _view.SetMetaIconActive(true);
            _view.SetDescription(_levelToBuy.description);
            _view.SetPrice(_levelToBuy.price.ToString(), canBuy ?  Color.cornflowerBlue : Color.red);
            _view.SetInteractable(canBuy);
        }

        public void Dispose() => _view.OnBuyClicked -= OnBuyClicked;
    }
}