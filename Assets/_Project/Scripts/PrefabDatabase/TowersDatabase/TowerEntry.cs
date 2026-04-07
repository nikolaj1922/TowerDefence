using System;
using _Project.Scripts.Towers;

namespace _Project.Scripts.PrefabDatabase.TowersDatabase
{
    [Serializable]
    public class TowerEntry
    {
        public TowerType type;
        public Tower prefab;
    }
}