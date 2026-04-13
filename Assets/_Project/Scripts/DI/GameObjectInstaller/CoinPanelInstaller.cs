using Zenject;
using UnityEngine;
using _Project.Scripts.UI.CoinCounter;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CoinPanelInstaller : MonoInstaller
    {
        [SerializeField] private CoinCounterPanel _coinPanel;
     
        public override void InstallBindings()
        {
            Container.Bind<CoinCounterPanel>().FromInstance(_coinPanel).AsSingle();
            Container.BindInterfacesAndSelfTo<CoinCounterController>().AsSingle();
        }
    }
}