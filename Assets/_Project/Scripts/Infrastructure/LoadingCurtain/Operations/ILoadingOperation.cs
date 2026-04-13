using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public interface ILoadingOperation
    {
        string Description { get; }
        UniTask Load();
    }
}