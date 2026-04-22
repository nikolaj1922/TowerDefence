using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Infrastructure.LoadingCurtain
{
    public class LoadingCurtainView : MonoBehaviour
    {
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private TextMeshProUGUI _description;
        
        public void Show() => gameObject.SetActive(true);
        
        public void Hide() => gameObject.SetActive(false);
        
        public void SetDescription(string description) => _description.text = description;

        public void Reset()
        {
            _loadingSlider.value = 0;
            _description.text = "";
        }

        public void DrawProgress(LoadingCurtainModel model) => _loadingSlider.value = model.CompletedOperations / model.OperationCount;
    }
}