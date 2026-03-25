using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.Tower
{
    public class TowerPlacement : ITickable
    {
        public event Action<Vector3> OnPlaceClicked;
        
        private readonly Camera _camera;
        private readonly LayerMask _towerLayer;
        private readonly LayerMask _groundLayer;

        private bool _canSelectPlace;

        public TowerPlacement(Camera camera, LayerMask towerLayer, LayerMask groundLayer)
        {
            _camera = camera;
            _towerLayer = towerLayer;
            _groundLayer = groundLayer;
        }

        public void Tick()
        {
            if (!_canSelectPlace) return; 
            
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
                TryPlaceTower(Input.mousePosition);
#else
            if(Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                    
                if(touch.phase == TouchPhase.Began)
                    TryPlaceTower(touch.position);
            }
#endif
        }

        public void DisableTowerPlacement() => _canSelectPlace = false;
        
        public void EnableTowerPlacement() => _canSelectPlace = true; 

        private void TryPlaceTower(Vector3 position)
        {
            if (!CanPlaceTower(position))
                return;

            GetGroundTowerPosition(position);
        }
        
        private bool CanPlaceTower(Vector3 mousePosition)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            return !Physics.Raycast(
                ray,
                out RaycastHit hit,
                GameConstants.TOWER_PLACEMENT_RAYCAST_DISTANCE,
                _towerLayer);
        }

        private void GetGroundTowerPosition(Vector3 mousePosition)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(
                    ray, 
                    out RaycastHit groundHit,
                    GameConstants.TOWER_PLACEMENT_RAYCAST_DISTANCE,
                    _groundLayer))
                OnPlaceClicked?.Invoke(groundHit.point);
        }
    }
}