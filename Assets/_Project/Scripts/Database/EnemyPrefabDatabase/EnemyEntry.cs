using System;
using _Project.Scripts.Enemy;

namespace _Project.Scripts.Database.EnemyPrefabDatabase
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public Enemy.Enemy prefab;
    }
}