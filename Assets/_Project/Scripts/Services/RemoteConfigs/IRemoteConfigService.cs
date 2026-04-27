using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.RemoteConfigs
{
    public interface IRemoteConfigService
    {
        bool TryGetConfig<T>(string key, out T result);
        UniTask LoadRemoteConfig();
    }
}