using Zenject;
using UnityEngine;
using _Project.Scripts.Weapons;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Towers
{
    public class Tower : MonoBehaviour, IWeaponMountOwner
    {
        [SerializeField] private GameObject _occupiedArea;
        [field: SerializeField] public Transform WeaponPoint { get; private set; }
        
        private TowerDTO _dto;
        public Weapon Weapon { get; private set; }
        
        [Inject]
        public void Construct(TowerDTO dto) => _dto = dto;

        private void Awake() => _occupiedArea.transform.localScale = GetOccupiedScale;

        public void SetWeapon(Weapon weapon) => Weapon = weapon;
        
        private Vector3 GetOccupiedScale 
            => new(_dto.occupiedRadius, _occupiedArea.transform.localScale.y, _dto.occupiedRadius);
    }
}