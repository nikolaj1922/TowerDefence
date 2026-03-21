using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Logic.Health
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _healthBarImage;

        public void SetFill(float newFill) => _healthBarImage.fillAmount = newFill;
    }
}