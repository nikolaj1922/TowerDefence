using _Project.Scripts.Database.Enemies;
using _Project.Scripts.Database.Game;
using _Project.Scripts.Database.Towers;
using _Project.Scripts.Database.Upgrades;
using _Project.Scripts.Database.Waves;
using _Project.Scripts.Database.Weapons;
using Zenject;
using _Project.Scripts.Infrastructure.LoadingCurtain;
using _Project.Scripts.Infrastructure.LoadingCurtain.PipelineFactory;
using _Project.Scripts.Services.RemoteConfigs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Game
{
    public class GameBoostrapper : IInitializable
    {
        private WavesDatabase _wavesDatabase;
        private UpgradeDatabase _upgradeDatabase;
        private GameDatabase _gameDatabase;
        private EnemyDatabase _enemyDatabase;
        private TowerDatabase _towerDatabase;
        private WeaponDatabase _weaponDatabase;
        private LoadingCurtainPresenter _loadingCurtainPresenter;
        private ILoadingPipelineFactory _loadingPipelineFactory;
        private IRemoteConfigService _remoteConfigService;
        private Camera _camera;

        [Inject]
        public void Construct(
            UpgradeDatabase upgradeDatabase,
            WavesDatabase wavesDatabase,
            GameDatabase gameDatabase,
            WeaponDatabase weaponDatabase,
            EnemyDatabase enemyDatabase,
            TowerDatabase towerDatabase,
            IRemoteConfigService remoteConfigService,
            LoadingCurtainPresenter loadingCurtainPresenter, 
            ILoadingPipelineFactory loadingPipelineFactory,
            Camera camera
            )
        {
            _camera = camera;
            _remoteConfigService = remoteConfigService;
            _wavesDatabase = wavesDatabase;
            _upgradeDatabase = upgradeDatabase;
            _gameDatabase = gameDatabase;
            _towerDatabase = towerDatabase;
            _enemyDatabase = enemyDatabase;
            _weaponDatabase = weaponDatabase;
            _loadingPipelineFactory = loadingPipelineFactory;
            _loadingCurtainPresenter = loadingCurtainPresenter;
        }

        public void Initialize()
        {
            _camera.aspect = (float)Screen.width / Screen.height;
            LoadInitialAssets().Forget();
        }

        private async UniTask LoadInitialAssets()
        {
            await _loadingCurtainPresenter.StartLoadingOperations(_loadingPipelineFactory.StartGamePipeline());
            InitDatabases();
        }

        private void InitDatabases()
        {
            _upgradeDatabase.LoadConfigs(_remoteConfigService);
            _gameDatabase.LoadConfig(_remoteConfigService);
            _enemyDatabase.LoadConfig(_remoteConfigService);
            _towerDatabase.LoadConfig(_remoteConfigService);
            _weaponDatabase.LoadConfig(_remoteConfigService);
            _wavesDatabase.LoadConfig(_remoteConfigService);
        }
    }
}