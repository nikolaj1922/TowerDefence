using System;
using _Project.Scripts.Tower;

namespace _Project.Scripts.Database.TowersDatabase
{
    [Serializable]
    public class TowerEntry
    {
        public TowerType type;
        public Tower.Tower prefab;
    }
}