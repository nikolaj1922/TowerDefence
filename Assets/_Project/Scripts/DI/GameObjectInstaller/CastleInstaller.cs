using _Project.Scripts.Database.Game;
using Zenject;
using UnityEngine;
using _Project.Scripts.UI.HealthBar;
using _Project.Scripts.Services.TowerUpgrade;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CastleInstaller : MonoInstaller
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private GameDatabase _gameDatabase;
        private ITowerUpgradeService _towerUpgradeService;

        [Inject]
        public void Construct(
            GameDatabase gameDatabase,
            ITowerUpgradeService towerUpgradeService
        )
        {
            _towerUpgradeService = towerUpgradeService;
            _gameDatabase = gameDatabase;
        }
        
        public override void InstallBindings()
        {
            float castleHp = 
                _gameDatabase.GetConfig().castleHealth *
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