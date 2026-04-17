using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public interface IEnemySpawner
    {
        Vector3 GetRandomSpawnPoint(float offset);
    }
}