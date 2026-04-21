using UnityEngine;

namespace _Project.Scripts.Towers
{
    public interface ITowerService
    {
        Tower Create(TowerType towerType, Vector3 position, float damageMultiplier, float attackSpeedMultiplier);
        void CreateAndPurchase(TowerType towerType, Vector3 position, int coinPrice, float damageMultiplier, float attackSpeedMultiplier);
    }
}