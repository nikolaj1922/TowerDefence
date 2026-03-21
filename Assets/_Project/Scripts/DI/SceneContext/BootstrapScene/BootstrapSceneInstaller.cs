using Zenject;
using _Project.Scripts.Infrastructure.Game;

namespace _Project.Scripts.DI.SceneContext.BootstrapScene
{
    public class BootstrapSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameBoostrapper>().AsSingle();
        }
    }
}