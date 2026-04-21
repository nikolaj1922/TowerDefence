using System.Collections.Generic;
using _Project.Scripts.Infrastructure.LoadingCurtain.Operations;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public interface ILoadingCurtainFactory
    {
        UniTaskVoid Create(Queue<ILoadingOperation> loadingOperations);
    }
}