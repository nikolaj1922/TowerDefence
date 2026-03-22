using UnityEngine;
using _Project.Scripts.UI.HealthBar;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CastleInstaller : TowerInstaller
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            HealthModel healthModel = new HealthModel(100f);
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.Bind<HealthController>().AsSingle();
            Container.Bind<HealthModel>().FromInstance(healthModel).AsSingle();
        }
    }
}