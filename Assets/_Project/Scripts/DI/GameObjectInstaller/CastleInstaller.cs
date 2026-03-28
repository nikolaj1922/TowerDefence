using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.UI.HealthBar;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CastleInstaller : TowerInstaller
    {
        [SerializeField] private HealthBarView _healthBarView;
        private HealthModel _healthModel;

        [Inject]
        public void Construct([Inject(Id = GameConstants.CASTLE_HEALTH_MODEL_INJECT_ID)] HealthModel castleHealthModel) => 
            _healthModel = castleHealthModel;
        
        public override void InstallBindings()
        {
            base.InstallBindings();
            
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle().WithArguments(_healthModel);
        }
    }
}