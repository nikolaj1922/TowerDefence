using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerPanelView : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.2f;
        
        private event Action<int> OnCreateTower;

        [SerializeField] private Button _closePanelButton;
        private readonly List<CreateTowerButtonView> _buttonViews = new();
        private readonly List<CreateTowerButtonPresenter> _buttonPresenters = new();
        
        [field: SerializeField] public RectTransform PanelRectTransform { get; private set; }
        private Vector3 _createTowerPosition;
        
        private void Awake() => _closePanelButton.onClick.AddListener(HidePanel);

        private void OnDestroy()
        {
            _closePanelButton.onClick.RemoveListener(HidePanel);
            
            foreach (CreateTowerButtonView buttonView in _buttonViews)
                buttonView.OnCreateTower -= OnCreateTower;

            foreach (CreateTowerButtonPresenter buttonPresenter in _buttonPresenters)
            {
                buttonPresenter.Dispose();
                buttonPresenter.OnSuccessCreateTower -= HidePanel;
            }
                    
        }

        public Vector3 GetCreateTowerPosition() => _createTowerPosition;
        
        public void Initialize(Action<int> onCreateTower) => OnCreateTower = onCreateTower;

        public void RegisterButton(CreateTowerButtonView buttonView, CreateTowerButtonPresenter presenter)
        {
            presenter.OnSuccessCreateTower += HidePanel;
            buttonView.OnCreateTower += OnCreateTower;
            
            _buttonPresenters.Add(presenter);
            _buttonViews.Add(buttonView);
        }

        public void HidePanel()
        {
            PanelRectTransform
                .DOAnchorPos(new Vector2(0, -GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }

        public void ShowPanel(Vector3 clickPosition)
        {
            _createTowerPosition = clickPosition;
            PanelRectTransform
                .DOAnchorPos(new Vector2(0, GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }
    }
}

