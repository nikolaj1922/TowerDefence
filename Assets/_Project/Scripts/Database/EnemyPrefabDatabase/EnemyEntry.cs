using System;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.Behaviour;

namespace _Project.Scripts.Database.EnemyPrefabDatabase
{
    [Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public Enemy prefab;
    }
}