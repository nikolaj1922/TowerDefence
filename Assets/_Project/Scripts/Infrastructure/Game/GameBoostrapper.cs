using Zenject;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private LoadingCurtainPresenter _loadingCurtainPresenter;
        private ILoadingPipelineFactory _loadingPipelineFactory;
        private Camera _camera;

        [Inject]
        public void Construct(
            LoadingCurtainPresenter loadingCurtainPresenter, 
            ILoadingPipelineFactory loadingPipelineFactory,
            Camera camera
            )
        {
            _camera = camera;
            _loadingPipelineFactory = loadingPipelineFactory;
            _loadingCurtainPresenter = loadingCurtainPresenter;
        }

        public void Initialize()
        {
            _camera.aspect = (float)Screen.width / Screen.height;
            _loadingCurtainPresenter.StartLoadingOperations(_loadingPipelineFactory.StartGamePipeline()).Forget();
        }
    }
}