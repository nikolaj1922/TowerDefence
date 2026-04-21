using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.Towers;
using _Project.Scripts.Towers;

namespace _Project.Scripts.DI.GameObjectInstaller
{
    public class TowerInstaller : MonoInstaller
    {
        [SerializeField] private TowerType _towerType;
        
        private TowerConfigsDatabase _configsDatabase;
        
        [Inject]
        public void Construct(TowerConfigsDatabase database) => _configsDatabase = database;
        
        public override void InstallBindings()
        {
            TowerConfig config = _configsDatabase.Get(_towerType);
            Container.Bind<TowerConfig>().FromInstance(config).AsSingle();
        }
    }
}
