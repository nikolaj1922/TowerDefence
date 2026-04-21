using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Database
{
    public interface IDatabase
    {
        UniTask Load();
    }
}