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
        
        private EnemyDatabase _enemyDatabase;
        
        [Inject]
        public void Construct(EnemyDatabase enemyDatabase) => _enemyDatabase = enemyDatabase;
        
        public override void InstallBindings()
        {
            EnemyDTO dto = _enemyDatabase.GetConfig(_enemyType);
            Container.Bind<EnemyDTO>().FromInstance(dto).AsSingle();
            Container.Bind<HealthModel>().FromMethod(_ => new HealthModel(dto.health)).AsSingle();
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle();
        }
    }
}