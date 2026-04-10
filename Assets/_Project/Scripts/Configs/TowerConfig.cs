using _Project.Scripts.Towers;
using UnityEngine;
using _Project.Scripts.Weapons;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Tower Config", fileName = "Tower")]
    public class TowerConfig : ScriptableObject
    {
        [field: SerializeField] public TowerType TowerType { get;  private set; }
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public bool CanBuild { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int CoinPrice { get; private set; }
        [field: SerializeField] public float OccupiedRadius { get; private set; }
    }
}