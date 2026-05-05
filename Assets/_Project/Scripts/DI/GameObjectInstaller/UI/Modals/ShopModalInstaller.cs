using _Project.Scripts.UI.Modals.ShopModal;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DI.GameObjectInstaller.UI.Modals
{
    public class ShopModalInstaller : MonoInstaller
    {
        [SerializeField] private ShopModalView _view;
        
        public override void InstallBindings()
        {
            Container.Bind<ShopModalView>().FromInstance(_view).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopModalPresenter>().AsSingle();
        }
    }
}