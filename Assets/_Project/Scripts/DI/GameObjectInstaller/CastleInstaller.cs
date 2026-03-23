using Zenject;
using UnityEngine;
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
            
            HealthModel healthModel = new HealthModel(_levelRepository.LevelConfig.castleHealth);
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle();
            Container.Bind<HealthModel>().FromInstance(healthModel).AsSingle();
        }
    }
}