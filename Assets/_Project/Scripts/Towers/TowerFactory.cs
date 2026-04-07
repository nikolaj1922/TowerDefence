using _Project.Scripts.PrefabDatabase.TowersDatabase;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Towers
{
    public class TowerFactory
    {
        private const float CASTLE_INIT_HEIGHT = 0.2f;
        
        private readonly DiContainer _container;
        private readonly TowerPrefabsDatabase _towerPrefabsDatabase;

        public TowerFactory(
            DiContainer container,
            TowerPrefabsDatabase towerPrefabsDatabase
        )
        {
            _container = container;
            _towerPrefabsDatabase = towerPrefabsDatabase;
        }

        public Tower CreateTower(TowerType type, Vector3 position)
        {
            GameObject castleObject = 
                _container.InstantiatePrefab(
                    _towerPrefabsDatabase.Get(type), 
                    new Vector3(position.x, CASTLE_INIT_HEIGHT, position.z), 
                    Quaternion.identity, 
                    null);
            
            return castleObject.GetComponent<Tower>();
        }
    }
}