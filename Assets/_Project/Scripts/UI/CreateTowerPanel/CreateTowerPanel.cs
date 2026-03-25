using System;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.UI.CreateTowerPanel
{
    public class CreateTowerPanel : MonoBehaviour
    {
        public event Action OnShowPanel;
        public event Action OnHidePanel;
        
        private RectTransform _rectTransform;
        public Vector3 CreateTowerPosition { get; private set; }

        [SerializeField] private Button _closePanelButton;
        
        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void Start() => _closePanelButton.onClick.AddListener(HidePanel);

        private void OnDestroy() =>  _closePanelButton.onClick.RemoveListener(HidePanel);

        public void HidePanel()
        {
            _rectTransform.anchoredPosition = new Vector2(0, -GameConstants.TOWER_PANEL_OFFSET);
            OnHidePanel?.Invoke();
        }

        public void ShowPanel(Vector3 clickPosition)
        {
            CreateTowerPosition = clickPosition;
            _rectTransform.anchoredPosition = new Vector2(0, GameConstants.TOWER_PANEL_OFFSET);
            OnShowPanel?.Invoke();
        }
    }
}

