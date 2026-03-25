using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.UI.CoinCounter;
using UnityEngine.Serialization;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CoinPanelInstaller : MonoInstaller
    {
        [FormerlySerializedAs("_coinView")] [SerializeField] private CoinCounterPanel _coinPanel;
     
        public override void InstallBindings()
        {
            Container.Bind<CoinCounterPanel>().FromInstance(_coinPanel).AsSingle();
            Container.BindInterfacesAndSelfTo<CoinCounterController>().AsSingle();
        }
    }
}