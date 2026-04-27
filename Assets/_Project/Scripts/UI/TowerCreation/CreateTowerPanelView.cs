using System;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Towers;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerPanelView : MonoBehaviour
    {
        public event Action<int, TowerType> OnTowerButtonClick;
        
        private const float ANIMATION_DURATION = 0.2f;

        private IInstantiator _instantiator;
        private CoinCounterModel _coinCounterModel;
        private List<CreateTowerButtonView> _buttons;
        
        [SerializeField] private Button _closePanelButton;
        [SerializeField] private RectTransform _panelRectTransform;
        [SerializeField] private CreateTowerButtonView _createTowerButtonView;

        [Inject]
        public void Construct(IInstantiator instantiator, CoinCounterModel coinCounterModel)
        {
            _coinCounterModel = coinCounterModel;
            _instantiator = instantiator;
        }
        
        private void Awake() => _closePanelButton.onClick.AddListener(HidePanel);

        private void OnDestroy()
        {
            _closePanelButton.onClick.RemoveListener(HidePanel);

            foreach (var button in _buttons)
            {
                button.OnCreateTower -= OnTowerButtonClick;
                _coinCounterModel.OnCoinChanged -= button.Draw;
            }
        }

        public void DrawTowerButtons(TowerDTO[] towerConfigs)
        {
            _buttons = new List<CreateTowerButtonView>();
            
            foreach (TowerDTO towerConfig in towerConfigs)
                DrawButton(towerConfig);
        }

        public void HidePanel()
        {
            _panelRectTransform
                .DOAnchorPos(new Vector2(0, -GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }

        public void ShowPanel()
        {
            _panelRectTransform
                .DOAnchorPos(new Vector2(0, GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }
        
        private void DrawButton(TowerDTO towerConfig)
        {
            CreateTowerButtonView towerButtonView =
                _instantiator.InstantiatePrefabForComponent<CreateTowerButtonView>(
                    _createTowerButtonView, 
                    _panelRectTransform
                );
            
            towerButtonView.Initialize(towerConfig.iconAddress, towerConfig.coinPrice, towerConfig.type);
            towerButtonView.OnCreateTower += OnTowerButtonClick;
            _coinCounterModel.OnCoinChanged += towerButtonView.Draw;
            
            _buttons.Add(towerButtonView);
        }
    }
}

