using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SaveLoad
{
    public interface ISaveLoad
    {
        PlayerProgress PlayerProgress { get; }
        
        UniTask SaveProgress();
        UniTask LoadProgress();
        UniTask InitializeRemoteSave();
    }
}