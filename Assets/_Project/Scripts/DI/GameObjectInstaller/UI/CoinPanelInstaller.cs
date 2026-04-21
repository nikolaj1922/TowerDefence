using _Project.Scripts.UI.CoinCounter;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.DI.GameObjectInstaller.UI
{
    public class CoinPanelInstaller : MonoInstaller
    {
        [SerializeField] private CoinCounterView _coinView;
     
        public override void InstallBindings()
        {
            Container.Bind<CoinCounterView>().FromInstance(_coinView).AsSingle();
            Container.BindInterfacesAndSelfTo<CoinCounterPresenter>().AsSingle();
        }
    }
}