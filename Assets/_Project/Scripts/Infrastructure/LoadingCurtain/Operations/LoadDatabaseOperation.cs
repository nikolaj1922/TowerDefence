using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadDatabaseOperation : ILoadingOperation
    {
        private readonly IEnumerable<UniTask> _databases;
        private readonly string _description;

        public LoadDatabaseOperation(IEnumerable<UniTask> databases, string description)
        {
            _description = description;
            _databases = databases;
        }

        public string Description => _description;

        public async UniTask Load() => await UniTask.WhenAll(_databases);
    }
}