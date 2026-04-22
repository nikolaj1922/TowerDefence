using Zenject;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private LoadingCurtainPresenter _loadingCurtainPresenter;
        private ILoadingPipelineFactory _loadingPipelineFactory;

        [Inject]
        public void Construct(
            LoadingCurtainPresenter loadingCurtainPresenter, ILoadingPipelineFactory loadingPipelineFactory)
        {
            _loadingPipelineFactory = loadingPipelineFactory;
            _loadingCurtainPresenter = loadingCurtainPresenter;
        }

        public void Initialize() =>
            _loadingCurtainPresenter.StartLoadingOperations(_loadingPipelineFactory.StartGamePipeline()).Forget();

    }
}