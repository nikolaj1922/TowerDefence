using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;
using UnityEngine;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerPanelModel
    {
        public TowerDTO[] BuildableTowerConfigs { get; private set; }
        public Vector3 TowerPosition { get; private set; }
        public List<CreateTowerButtonView> TowerButtons { get; } =  new();

        public CreateTowerPanelModel(TowerDTO[] buildableTowerConfigs) =>
            BuildableTowerConfigs = buildableTowerConfigs; 
        
        public void SetTowerPosition(Vector3 pos) => TowerPosition = pos;
        
        public void RegisterButton(CreateTowerButtonView button) => TowerButtons.Add(button);
        
        public void ClearButtons() => TowerButtons.Clear();
    }
}