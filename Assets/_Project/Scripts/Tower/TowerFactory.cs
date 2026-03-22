using Zenject;
using UnityEngine;
using _Project.Scripts.Database.TowersDatabase;

namespace _Project.Scripts.Tower
{
    public class TowerFactory
    {
        private readonly DiContainer _container;
        private readonly TowerPrefabsDatabase _towerPrefabsDatabase;
        
        public TowerFactory(TowerPrefabsDatabase towerPrefabsDatabase, DiContainer container)
        {
            _container = container;
            _towerPrefabsDatabase = towerPrefabsDatabase;
        } 
        
        public Tower CreateTower(TowerType type, Vector3 position)
        {
            GameObject castleObject = 
                _container.InstantiatePrefab(_towerPrefabsDatabase.Get(type), position, Quaternion.identity, null);
            Tower tower = castleObject.GetComponent<Tower>();

            return tower;
        }
    }
}