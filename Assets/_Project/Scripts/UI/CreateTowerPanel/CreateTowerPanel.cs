using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.UI.CreateTowerPanel
{
    public class CreateTowerPanel : MonoBehaviour
    {
        private RectTransform _rectTransform;
        public Vector3 CreateTowerPosition { get; private set; }

        [SerializeField] private Button _closePanelButton;
        
        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void Start() => _closePanelButton.onClick.AddListener(HidePanel);

        private void OnDestroy() =>  _closePanelButton.onClick.RemoveListener(HidePanel);

        public void HidePanel()
        {
            _rectTransform.DOAnchorPos(new Vector2(0, -GameConstants.PANEL_OFFSET), 0.2f)
                .SetEase(Ease.InOutQuad);
        }

        public void ShowPanel(Vector3 clickPosition)
        {
            CreateTowerPosition = clickPosition;
            _rectTransform.DOAnchorPos(new Vector2(0, GameConstants.PANEL_OFFSET), 0.2f)
                .SetEase(Ease.InOutQuad);
        }
    }
}

