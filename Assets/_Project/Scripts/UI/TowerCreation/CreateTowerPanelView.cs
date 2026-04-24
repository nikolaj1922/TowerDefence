using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.UI.TowerCreation.CreateTowerButton;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.TowerCreation
{
    public class CreateTowerPanelView : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.2f;

        [SerializeField] private Button _closePanelButton;
        
        [field: SerializeField] public RectTransform PanelRectTransform { get; private set; }
        [field: SerializeField] public CreateTowerButtonView CreateTowerButtonView { get; private set; }
        
        
        private void Awake() => _closePanelButton.onClick.AddListener(HidePanel);

        private void OnDestroy() => _closePanelButton.onClick.RemoveListener(HidePanel);

        public void HidePanel()
        {
            PanelRectTransform
                .DOAnchorPos(new Vector2(0, -GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }

        public void ShowPanel()
        {
            PanelRectTransform
                .DOAnchorPos(new Vector2(0, GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }
    }
}

