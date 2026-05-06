using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.NetworkChecker
{
    public interface INetworkChecker
    {
        UniTask<bool> CheckNetwork();
    }
}