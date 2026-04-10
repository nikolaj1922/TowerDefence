using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Enemies;
using _Project.Scripts.UI.HealthBar;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private HealthBarView _healthBarView;
        
        private EnemyConfigsRepository _enemyRepository;
        
        [Inject]
        public void Construct(EnemyConfigsRepository enemyRepository) => _enemyRepository = enemyRepository;
        
        public override void InstallBindings()
        {
            EnemyConfig config = _enemyRepository.Get(_enemyType);
            Container.Bind<EnemyConfig>().FromInstance(config).AsSingle();
            Container.Bind<HealthModel>().FromMethod(_ => new HealthModel(config.Health)).AsSingle();
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle();
        }
    }
}