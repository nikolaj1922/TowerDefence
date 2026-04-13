using _Project.Scripts.ConfigRepositories;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.LoadingCurtain.Operations
{
    public class LoadConfigsOperation : ILoadingOperation
    {
        private readonly GameRepository _gameRepository;
        private readonly EnemyConfigsRepository _enemyConfigsRepository;
        private readonly TowerConfigsRepository _towerConfigsRepository;
        private readonly WeaponConfigsRepository _weaponConfigsRepository;

        public LoadConfigsOperation(
            WeaponConfigsRepository weaponConfigsRepository,
            EnemyConfigsRepository enemyConfigsRepository,
            TowerConfigsRepository towerConfigsRepository,
            GameRepository gameRepository
            )
        {
            _gameRepository = gameRepository;
            _towerConfigsRepository = towerConfigsRepository;
            _enemyConfigsRepository = enemyConfigsRepository;
            _weaponConfigsRepository = weaponConfigsRepository;
        }
        
        public string Description => "Load game configs";
        public async UniTask Load()
        {
            await UniTask.WhenAll(
                _weaponConfigsRepository.Load(),
                _enemyConfigsRepository.Load(),
                _towerConfigsRepository.Load(),
                _gameRepository.Load()
                );
        }
    }
}