using _Project.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerPanelModel
    {
        public TowerDTO[] BuildableTowerConfigs { get; private set; }
        public Vector3 TowerPosition { get; private set; }

        public CreateTowerPanelModel(TowerDTO[] buildableTowerConfigs) =>
            BuildableTowerConfigs = buildableTowerConfigs; 
        
        public void SetTowerPosition(Vector3 pos) => TowerPosition = pos;
    }
}