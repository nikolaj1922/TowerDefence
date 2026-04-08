using Zenject;
using UnityEngine;
using _Project.Scripts.PrefabDatabase.TowersDatabase;

namespace _Project.Scripts.Towers
{
    public class TowerFactory
    {
        private const float CASTLE_INIT_HEIGHT = 0.2f;
        
        private readonly IInstantiator _instantiator;
        private readonly TowerPrefabsDatabase _towerPrefabsDatabase;

        public TowerFactory(
            IInstantiator instantiator,
            TowerPrefabsDatabase towerPrefabsDatabase
        )
        {
            _instantiator = instantiator;
            _towerPrefabsDatabase = towerPrefabsDatabase;
        }

        public Tower CreateTower(TowerType type, Vector3 position)
        {
            
            GameObject castleObject = 
                _instantiator.InstantiatePrefab(
                    _towerPrefabsDatabase.Get(type), 
                    new Vector3(position.x, CASTLE_INIT_HEIGHT, position.z), 
                    Quaternion.identity, 
                    null);
            
            return castleObject.GetComponent<Tower>();
        }
    }
}