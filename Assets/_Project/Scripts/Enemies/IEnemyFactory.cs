using System;

namespace _Project.Scripts.Enemies
{
    public interface IEnemyFactory
    {
        void CreateEnemy(EnemyType type, Action onDeath);
        void StopActiveEnemies();
    }
}