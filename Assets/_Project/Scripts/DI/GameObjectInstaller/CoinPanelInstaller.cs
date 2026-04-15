using Zenject;
using UnityEngine;
using _Project.Scripts.UI.CoinCounter;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CoinPanelInstaller : MonoInstaller
    {
        [SerializeField] private CoinCounterView _coinView;
     
        public override void InstallBindings()
        {
            Container.Bind<CoinCounterView>().FromInstance(_coinView).AsSingle();
            Container.BindInterfacesAndSelfTo<CoinCounterController>().AsSingle();
        }
    }
}