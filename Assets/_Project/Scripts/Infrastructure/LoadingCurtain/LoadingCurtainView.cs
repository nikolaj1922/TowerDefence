using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainView : MonoBehaviour
    {
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TextMeshProUGUI _description;

        public void SetDescription(string description) => _description.text = description;

        public void SetLoadingProgress(float progress) => _loadingSlider.value = progress;
        
        public void ResetLoadingProgress() => _loadingSlider.value = 0;
    }
}