using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.HealthBar
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _healthBarImage;

        public void SetFill(float newFill) => _healthBarImage.fillAmount = newFill;
    }
}