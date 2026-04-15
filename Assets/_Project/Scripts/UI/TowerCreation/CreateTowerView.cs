using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.Constants;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerView : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.2f;
        
        private Action<int> _onCreateTower;

        [SerializeField] private Button _closePanelButton;
        private readonly List<CreateTowerItemButton> _buttons = new();
        
        [field: SerializeField] public RectTransform PanelRectTransform { get; private set; }
        
        public Vector3 CreateTowerPosition { get; private set; }
        
        private void Start() => _closePanelButton.onClick.AddListener(HidePanel);

        private void OnDestroy()
        {
            _closePanelButton.onClick.RemoveListener(HidePanel);

            if (_onCreateTower == null)
                return;

            foreach (var button in _buttons)
            {
                if (button != null)
                    button.onCreateTower -= _onCreateTower;
            }
        }

        public void Initialize(Action<int> onCreateTower)
        {
            _onCreateTower = onCreateTower;
        }

        public void RegisterButton(CreateTowerItemButton button)
        {
            _buttons.Add(button);
            button.onCreateTower += _onCreateTower;
        }

        public void HidePanel()
        {
            PanelRectTransform
                .DOAnchorPos(new Vector2(0, -GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }

        public void ShowPanel(Vector3 clickPosition)
        {
            CreateTowerPosition = clickPosition;
            PanelRectTransform
                .DOAnchorPos(new Vector2(0, GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }
    }
}

