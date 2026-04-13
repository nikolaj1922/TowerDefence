using Zenject;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private readonly LoadingCurtainController _loadingCurtainController;
        private readonly LoadingPipelineFactory _loadingPipelineFactory;

        public GameBoostrapper(
            LoadingCurtainController loadingCurtainController,
            LoadingPipelineFactory loadingPipelineFactory)
        {
            _loadingCurtainController = loadingCurtainController;
            _loadingPipelineFactory = loadingPipelineFactory;
        }

        public void Initialize()
        {
            _loadingCurtainController.Run(_loadingPipelineFactory.GetStartGamePipeline()).Forget();
        }
    }
}