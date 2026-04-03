using System;
using _Project.Scripts.Enemy;

namespace _Project.Scripts.PrefabDatabase.EnemyDatabase
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public Enemy.Enemy prefab;
    }
}