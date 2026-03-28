using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Tower
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private GameObject _occupiedArea;
        private TowerConfig _config;
        protected Weapon.Weapon _weapon;

        [field: SerializeField] public Transform WeaponPoint { get; private set; }

        [Inject]
        public void ConstructBase(TowerConfig config) => _config = config;

        private void Awake()
        {
            _occupiedArea.transform.localScale = 
                new Vector3(_config.occupiedRadius, _occupiedArea.transform.localScale.y, _config.occupiedRadius);
        }

        public void SetWeapon(Weapon.Weapon weapon) => _weapon = weapon;
    }
}