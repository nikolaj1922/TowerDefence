using _Project.Scripts.Weapons;
using UnityEngine;

namespace _Project.Scripts.Towers
{
    public interface IWeaponMountOwner
    {
        Transform WeaponPoint { get; }
        void SetWeapon(Weapon weapon);
    }
}