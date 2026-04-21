using _Project.Scripts.UI.Modals.UpgradeModal;
using _Project.Scripts.UI.Modals.UpgradeModal.UpgradeButton;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DI.GameObjectInstaller.UI.Modals
{
    public class UpgradeModalInstaller : MonoInstaller
    {
        [SerializeField] private UpgradeModalView _view;
        [SerializeField] private UpgradeButtonView _upgradeButtonView;
        
        public override void InstallBindings()
        {
            Container.Bind<UpgradeModalView>().FromInstance(_view).AsSingle();
            Container.Bind<UpgradeButtonView>().FromInstance(_upgradeButtonView).AsSingle();
            Container.BindInterfacesAndSelfTo<UpgradeModalPresenter>().AsSingle();
        }   
    }
}