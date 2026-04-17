using UnityEngine;

namespace _Project.Scripts.Towers.Castle
{
    public interface ICastleInitializer
    {
        CastleTower CreateCastle(
            Vector3 position, 
            float damageMultiplier, 
            float attackSpeedMultiplier);
    }
}