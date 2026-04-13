using System;
using Zenject;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using _Project.Scripts.Logic.Level;
using _Project.Scripts.Services.Analytics;
using _Project.Scripts.Infrastructure.Constants;

namespace _Project.Scripts.Towers
{
    public class TowerPlacement : ITickable
    {
        public event Action<Vector3> OnPlaceClicked;
        
        private readonly Camera _camera;
        private readonly WaveManager _waveManager;
        private readonly LayerMask _groundLayer;
        private readonly LayerMask _towerOccupiedLayer;
        private readonly AnalyticsService _analyticsService;

        public TowerPlacement(
            Camera camera,
            AnalyticsService analyticsService,
            WaveManager waveManager,
            LayerMask towerOccupiedLayer, 
            LayerMask groundLayer)
        {
            _camera = camera;
            _waveManager = waveManager;
            _analyticsService = analyticsService;
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

            if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    GameConstants.TOWER_PLACEMENT_RAYCAST_DISTANCE,
                    _towerOccupiedLayer))
            {
                _analyticsService.BuildRejected("too_close_to_tower", _waveManager.CurrentWave);
                return;
            }
            
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