using UnityEngine;

namespace _Project.Scripts.Tower
{
    public class Tower : MonoBehaviour
    {
        protected Weapon.Weapon _weapon;
        
        [field: SerializeField] public Transform WeaponPoint { get; private set; }
        
        public void SetWeapon(Weapon.Weapon weapon) => _weapon = weapon;
    }
}