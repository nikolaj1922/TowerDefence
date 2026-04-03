using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.StartNextWavePanel;
using _Project.Scripts.UI.WaveCounter;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class WaveCounterPanelInstaller : MonoInstaller
    {
        [SerializeField] private WaveCounterPanel _waveCounterPanel;

        public override void InstallBindings()
        {
            Container.Bind<WaveCounterPanel>().FromInstance(_waveCounterPanel).AsSingle();
            Container.Bind<StartWaveModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartWavePresenter>().AsSingle();
        }
    }
}
