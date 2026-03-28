using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Infrastructure.GameConstants;
using DG.Tweening;

namespace _Project.Scripts.UI.WaveCounter
{
    public class WaveCounterPanel : MonoBehaviour
    {
        public event Action OnForceWaveClick;
        
        private const float ANIMATION_DURATION = 0.2f;

        [SerializeField] private Button _forceWaveButton;
        [SerializeField] private TextMeshProUGUI _waveTimerText;
        [SerializeField] private TextMeshProUGUI _waveCountText;
        private RectTransform _rectTransform;
        
        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        private void Start() => _forceWaveButton.onClick.AddListener(OnClick);

        private void OnDestroy() =>  _forceWaveButton.onClick.RemoveListener(OnClick);

        public void UpdateTimerText(int timer) => _waveTimerText.text = timer.ToString();

        public void HidePanel()
        {
            _rectTransform
                .DOAnchorPos(new Vector2(0, GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }
           
        public void ShowPanel(int waveCount)
        {
            _waveCountText.text = $"Wave {waveCount}";
            _rectTransform
                .DOAnchorPos(new Vector2(0, -GameConstants.PANEL_OFFSET), ANIMATION_DURATION)
                .SetEase(Ease.InOutQuad);
        }

        private void OnClick() => OnForceWaveClick?.Invoke();
    }
}