using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;

namespace _Project.Scripts.Tower
{
    [RequireComponent(typeof(SphereCollider))]
    public class Tower : MonoBehaviour
    {
        private SphereCollider _buildCollider;
        protected Weapon.Weapon _weapon;
        private TowerConfig _config;
        
        [field: SerializeField] public Transform WeaponPoint { get; private set; }

        [Inject]
        public void ConstructBase(TowerConfig config) => _config = config;

        private void Awake()
        {
            _buildCollider = GetComponent<SphereCollider>();
            _buildCollider.radius = _config.buildRadius;
        }

        public void SetWeapon(Weapon.Weapon weapon) => _weapon = weapon;
    }
}