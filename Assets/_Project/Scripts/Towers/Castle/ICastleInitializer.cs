using UnityEngine;

namespace _Project.Scripts.Towers.Castle
{
    public interface ICastleInitializer
    {
        ICastleTower CreateCastle(
            Vector3 position, 
            float damageMultiplier, 
            float attackSpeedMultiplier);
    }
}