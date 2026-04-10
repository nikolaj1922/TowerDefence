using UnityEngine;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Weapon Config", fileName = "Weapon")]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
    }
}