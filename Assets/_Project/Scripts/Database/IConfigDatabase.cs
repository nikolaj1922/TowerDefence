using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Database
{
    public interface IConfigDatabase
    {
        UniTask LoadConfigs();
        UniTask UnloadConfigs();
    }
}