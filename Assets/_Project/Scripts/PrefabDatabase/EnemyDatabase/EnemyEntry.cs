using System;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.PrefabDatabase.EnemyDatabase
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public Enemy prefab;
    }
}