using Zenject;
using _Project.Scripts.Infrastructure.Game;

namespace _Project.Scripts.DI.SceneContext.BootstrapScene
{
    public class BootstrapSceneInitializer : MonoInstaller
    {
        public override void InstallBindings() => BindBootstrapper();

        private void BindBootstrapper() =>
            Container.BindInterfacesTo<GameBoostrapper>().AsSingle();
    }
}