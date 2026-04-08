using Zenject;
using UnityEngine;
using _Project.Scripts.Weapons;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Towers;

namespace _Project.Scripts.Towers
{
    public class Tower : MonoBehaviour, IWeaponMountOwner
    {
        [SerializeField] private GameObject _occupiedArea;
        [field: SerializeField] public Transform WeaponPoint { get; private set; }
        
        private TowerConfig _config;
        public Weapon Weapon { get; private set; }
        
        [Inject]
        public void Construct(TowerConfig config) => _config = config;

        private void Awake() => _occupiedArea.transform.localScale = GetOccupiedScale;

        public void SetWeapon(Weapon weapon) => Weapon = weapon;
        
        private Vector3 GetOccupiedScale 
            => new(_config.OccupiedRadius, _occupiedArea.transform.localScale.y, _config.OccupiedRadius);
    }
}