using _Project.Scripts.Services.RemoteConfigs;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadRemoteConfigsOperation : ILoadingOperation
    {
        private readonly IRemoteConfigService _remoteConfigService;
        public string Description => "Load remote configs";

        public LoadRemoteConfigsOperation(IRemoteConfigService  remoteConfigService)
            => _remoteConfigService = remoteConfigService;

        public async UniTask Load() => await _remoteConfigService.LoadRemoteConfig();
    }
}