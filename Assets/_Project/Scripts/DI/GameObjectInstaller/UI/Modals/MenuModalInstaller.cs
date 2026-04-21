using _Project.Scripts.UI.Modals.MenuModal;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DI.GameObjectInstaller.UI.Modals
{
    public class MenuModalInstaller : MonoInstaller
    {
        [SerializeField] private MenuModalView _view;
        
        public override void InstallBindings()
        {
            Container.Bind<MenuModalView>().FromInstance(_view).AsSingle();
            Container.BindInterfacesAndSelfTo<MenuModalPresenter>().AsSingle();
        }
    }
}