using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.UI.HealthBar;
using _Project.Scripts.ConfigRepositories;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class CastleInstaller : MonoInstaller
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private GameRepository _gameRepository;

        [Inject]
        public void Construct(GameRepository gameRepository) => _gameRepository = gameRepository;
        
        public override void InstallBindings()
        {
            Container
                .Bind<HealthModel>()
                .FromMethod(_ => new HealthModel(_gameRepository.GameConfig.CastleHealth))
                .AsSingle();
            Container.Bind<HealthBarView>().FromInstance(_healthBarView).AsSingle();
            Container.BindInterfacesAndSelfTo<HealthController>().AsSingle();
        }
    }
}