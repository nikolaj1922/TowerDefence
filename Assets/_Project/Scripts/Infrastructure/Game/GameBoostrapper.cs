using Zenject;
using _Project.Scripts.Infrastructure.LoadingCurtain;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private ILoadingPipelineFactory _loadingPipelineFactory;
        private ILoadingCurtainFactory _loadingCurtainFactory;
        
        [Inject]
        public void Construct(
            ILoadingCurtainFactory loadingCurtainFactory,
            ILoadingPipelineFactory loadingPipelineFactory)
        {
            _loadingPipelineFactory = loadingPipelineFactory;
            _loadingCurtainFactory = loadingCurtainFactory;
        }

        public void Initialize() => 
            _loadingCurtainFactory
                .Create(_loadingPipelineFactory.StartGamePipeline())
                .Forget();
    }
}