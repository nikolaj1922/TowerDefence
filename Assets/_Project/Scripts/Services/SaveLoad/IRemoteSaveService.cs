using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SaveLoad
{
    public interface IRemoteSaveService
    {
        UniTask Initialize();
        UniTask Save(string progressJson);
        UniTask<PlayerProgress> Load();
    }
}