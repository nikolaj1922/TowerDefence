using _Project.Scripts.Configs;
using _Project.Scripts.Weapons;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private GameObject _occupiedArea;
        
        private TowerConfig _config;
        protected Weapon _weapon;

        [field: SerializeField] public Transform WeaponPoint { get; private set; }

        [Inject]
        public void ConstructBase(TowerConfig config) => _config = config;

        private void Awake()
        {
            _occupiedArea.transform.localScale = 
                new Vector3(_config.OccupiedRadius, _occupiedArea.transform.localScale.y, _config.OccupiedRadius);
        }

        public void SetWeapon(Weapon weapon) => _weapon = weapon;
    }
}