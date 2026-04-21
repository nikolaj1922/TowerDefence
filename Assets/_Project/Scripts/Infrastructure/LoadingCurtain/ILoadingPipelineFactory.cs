using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public interface ILoadingPipelineFactory
    {
        Queue<ILoadingOperation> StartGamePipeline();
        Queue<ILoadingOperation> LevelPipeline();
        Queue<ILoadingOperation> RestartLevelPipeline();
    }
}