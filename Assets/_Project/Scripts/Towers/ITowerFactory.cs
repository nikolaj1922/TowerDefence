using UnityEngine;

namespace _Project.Scripts.Towers
{
    public interface ITowerFactory
    {
        Tower CreateTower(TowerType type, Vector3 position);
    }
}