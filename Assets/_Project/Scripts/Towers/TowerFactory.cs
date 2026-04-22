using _Project.Scripts.Database.Towers;
using Zenject;
using UnityEngine;

namespace _Project.Scripts.Towers
{
    public class TowerFactory : ITowerFactory
    {
        private const float CASTLE_INIT_HEIGHT = 0.2f;
        
        private readonly IInstantiator _instantiator;
        private readonly TowerDatabase _towerDatabase;

        public TowerFactory(
            IInstantiator instantiator,
            TowerDatabase towerDatabase
        )
        {
            _instantiator = instantiator;
            _towerDatabase = towerDatabase;
        }

        public Tower CreateTower(TowerType type, Vector3 position)
        {
            GameObject castleObject = 
                _instantiator.InstantiatePrefab(
                    _towerDatabase.GetPrefab(type), 
                    new Vector3(position.x, CASTLE_INIT_HEIGHT, position.z), 
                    Quaternion.identity, 
                    null);
            
            return castleObject.GetComponent<Tower>();
        }
    }
}