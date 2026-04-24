using _Project.Scripts.UI.Modals.ContinueForAdsModal;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DI.GameObjectInstaller.UI.Modals
{
    public class ContinueForAdsModalInstaller : MonoInstaller
    {
        [SerializeField] private ContinueForAdsModalView _view;
        
        public override void InstallBindings()
        {
            Container.Bind<ContinueForAdsModalView>().FromInstance(_view).AsSingle();
            Container.BindInterfacesAndSelfTo<ContinueForAdsModalPresenter>().AsSingle();
        }
    }
}