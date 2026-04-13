using Zenject;
using UnityEngine;
using _Project.Scripts.UI.WaveCounter;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class WaveCounterPanelInstaller : MonoInstaller
    {
        [SerializeField] private WaveCounterView _waveCounterView;

        public override void InstallBindings()
        {
            Container.Bind<WaveCounterView>().FromInstance(_waveCounterView).AsSingle();
            Container.Bind<WaveCounterModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<WaveCounterPresenter>().AsSingle();
        }
    }
}
