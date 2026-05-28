using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SaveLoad
{
    public interface ISaveService
    {
        UniTask Initialize();
        UniTask Save(string progressJson);
        UniTask<PlayerProgress> Load();
        PlayerProgress GetProgress();
    }
}
