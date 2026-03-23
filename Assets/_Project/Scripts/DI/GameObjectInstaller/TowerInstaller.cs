using Zenject;
using UnityEngine;
using _Project.Scripts.Tower;
using _Project.Scripts.Configs;
using _Project.Scripts.Repositories;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class TowerInstaller : MonoInstaller
    {
        [SerializeField] private TowerType _towerType;
        
        private TowerConfigsRepository _configsRepository;
        
        [Inject]
        public void Construct(TowerConfigsRepository repository) => _configsRepository = repository;
        
        public override void InstallBindings()
        {
            TowerConfig config = _configsRepository.Get(_towerType);
            Container.Bind<TowerConfig>().FromInstance(config).AsSingle();
        }
    }
}
