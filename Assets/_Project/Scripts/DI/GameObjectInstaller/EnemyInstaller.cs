using Zenject;
using UnityEngine;
using _Project.Scripts.Enemy;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.Repositories;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private HealthBarView _healthBarView;

        private EnemyConfigsRepository _enemyRepository;
        
        [Inject]
        public void Construct(EnemyConfigsRepository enemyRepository)
        {
            _enemyRepository = enemyRepository;
        }
        
        public override void InstallBindings()
        {
            EnemyConfig config = _enemyRepository.ForEnemy(_enemyType);
            Container.Bind<EnemyConfig>().FromInstance(config).AsSingle();

            HealthModel healthModel = new HealthModel(config.health);
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.Bind<HealthController>().AsSingle();
            Container.Bind<HealthModel>().FromInstance(healthModel).AsSingle();
        }
    }
}