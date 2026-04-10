using System;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.Database.EnemyPrefabDatabase
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public Enemy prefab;
    }
}