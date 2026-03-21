using UnityEngine;

namespace _Project.Scripts.Logic.Health
{
    public class HealthController : MonoBehaviour, IDamagable
    {
        public HealthModel HealthModel { get; private set; }
        [SerializeField] private HealthBarView _healthBarView;

        private void OnDisable()
        {
            if (HealthModel != null)
                HealthModel.OnHealthChanged -= UpdateHealthBar;
        }
        
        public virtual void InitHealth(float health)
        {
            HealthModel = new HealthModel(health);
            HealthModel.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(HealthModel.CurrentHealth, HealthModel.MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"Health: {HealthModel.CurrentHealth} / {HealthModel.MaxHealth}");
            HealthModel.ChangeHealth(-damage);
        }

        private void UpdateHealthBar(float current, float max)
        {
            if (_healthBarView != null)
                _healthBarView.SetFill(current / max);
        }
    }
}