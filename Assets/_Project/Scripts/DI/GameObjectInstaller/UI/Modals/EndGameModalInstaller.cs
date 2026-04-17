using _Project.Scripts.UI.Modals.EndGameModal;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DI.GameObjectInstaller.UI.Modals
{
    public class EndGameModalInstaller : MonoInstaller
    {
        [SerializeField] private EndGameModalView _view;
        
        public override void InstallBindings()
        {
            Container.Bind<EndGameModalView>().FromInstance(_view).AsSingle();
            Container.BindInterfacesAndSelfTo<EndGameModalPresenter>().AsSingle();
        }
    }
}