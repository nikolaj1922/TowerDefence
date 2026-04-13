using _Project.Scripts.Services.SaveLoad;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadPlayerProgressOperation : ILoadingOperation
    {
        private readonly ISaveLoad _saveLoad;
        
        public LoadPlayerProgressOperation(ISaveLoad saveLoad) => _saveLoad = saveLoad;
        
        public string Description => "Load player progress";
        
        public async UniTask Load() => await _saveLoad.LoadProgress();
    }
}