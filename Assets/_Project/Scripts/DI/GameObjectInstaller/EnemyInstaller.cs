using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.Enemies;
using _Project.Scripts.Enemies;
using _Project.Scripts.UI.HealthBar;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private HealthBarView _healthBarView;
        
        private EnemyConfigsDatabase _enemyDatabase;
        
        [Inject]
        public void Construct(EnemyConfigsDatabase enemyDatabase) => _enemyDatabase = enemyDatabase;
        
        public override void InstallBindings()
        {
            EnemyConfig config = _enemyDatabase.Get(_enemyType);
            Container.Bind<EnemyConfig>().FromInstance(config).AsSingle();
            Container.Bind<HealthModel>().FromMethod(_ => new HealthModel(config.Health)).AsSingle();
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle();
        }
    }
}