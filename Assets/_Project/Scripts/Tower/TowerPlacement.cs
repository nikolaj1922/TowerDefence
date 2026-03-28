using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Infrastructure.GameConstants;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace _Project.Scripts.Tower
{
    public class TowerPlacement : ITickable
    {
        public event Action<Vector3> OnPlaceClicked;
        
        private readonly Camera _camera;
        private readonly LayerMask _groundLayer;
        private readonly LayerMask _towerOccupiedLayer;

        public TowerPlacement(Camera camera, LayerMask towerOccupiedLayer, LayerMask groundLayer)
        {
            _camera = camera;
            _towerOccupiedLayer = towerOccupiedLayer;
            _groundLayer = groundLayer;
        }

        public void Tick()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE
                if (Input.GetMouseButtonDown(0))
                {
                    if (IsPointerOverUIElement(Input.mousePosition))
                        return;

                    TryPlaceTower(Input.mousePosition);
                }
            #else
                if(Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    if(touch.phase == TouchPhase.Began)
                    {
                        if (UIHelper.IsPointerOverUIElement(touch.position))
                            return;

                        TryPlaceTower(touch.position);
                    }
                }
            #endif
        }
        
        private bool IsPointerOverUIElement(Vector2 screenPosition)
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }

        private void TryPlaceTower(Vector3 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);

            if (!Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    GameConstants.TOWER_PLACEMENT_RAYCAST_DISTANCE,
                    _towerOccupiedLayer))
                PlaceTower(position);
        }
        
        private void PlaceTower(Vector3 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);
            
            if (Physics.Raycast(
                    ray, 
                    out RaycastHit groundHit,
                    GameConstants.TOWER_PLACEMENT_RAYCAST_DISTANCE,
                    _groundLayer))
                OnPlaceClicked?.Invoke(groundHit.point);
        }
    }
}