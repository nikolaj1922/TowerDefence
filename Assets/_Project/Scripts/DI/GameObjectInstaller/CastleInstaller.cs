using Zenject;
using UnityEngine;
using _Project.Scripts.UI.HealthBar;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CastleInstaller : MonoInstaller
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private GameRepository _gameRepository;
        private TowerUpgradeService _towerUpgradeService;

        [Inject]
        public void Construct(
            GameRepository gameRepository,
            TowerUpgradeService towerUpgradeService
        )
        {
            _towerUpgradeService = towerUpgradeService;
            _gameRepository = gameRepository;
        }
        
        public override void InstallBindings()
        {
            float castleHp = 
                _gameRepository.GameConfig.CastleHealth *
                _towerUpgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_HP_ID);
            
            Container
                .Bind<HealthModel>()
                .FromMethod(_ => new HealthModel(castleHp))
                .AsSingle();
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle();
        }
    }
}