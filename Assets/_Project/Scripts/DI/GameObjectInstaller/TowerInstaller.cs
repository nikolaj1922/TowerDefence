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
        
        private TowerDatabase _towerDatabase;
        
        [Inject]
        public void Construct(TowerDatabase towerDatabase) => _towerDatabase = towerDatabase;
        
        public override void InstallBindings()
        {
            TowerConfig config = _towerDatabase.GetConfig(_towerType);
            Container.Bind<TowerConfig>().FromInstance(config).AsSingle();
        }
    }
}
