using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.Repositories;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CastleInstaller : TowerInstaller
    {
        [SerializeField] private HealthBarView _healthBarView;

        private LevelRepository _levelRepository;
        
        [Inject]
        private void Construct(LevelRepository levelRepository) =>  _levelRepository = levelRepository;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            LevelConfig levelConfig = _levelRepository.LevelConfig;
            
            HealthModel healthModel = new HealthModel(levelConfig.castleHealth);
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.Bind<HealthController>().AsSingle();
            Container.Bind<HealthModel>().FromInstance(healthModel).AsSingle();
        }
    }
}