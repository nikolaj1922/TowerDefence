using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Database
{
    public interface IPrefabDatabase
    {
        UniTask LoadPrefabs();
        UniTask UnloadPrefabs();
    }
}