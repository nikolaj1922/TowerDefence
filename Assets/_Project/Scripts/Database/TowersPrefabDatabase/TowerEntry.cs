using System;
using _Project.Scripts.Towers;

namespace _Project.Scripts.Database.TowersPrefabDatabase
{
    [Serializable]
    public class TowerEntry
    {
        public TowerType type;
        public Tower prefab;
    }
}