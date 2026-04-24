using System;
using UnityEngine;

namespace _Project.Scripts.UI.HealthBar
{
    public class HealthModel
    {
        public event Action<float> OnHealthChanged;

        private float _currentHealth;
        public float MaxHealth { get; }

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if (Mathf.Approximately(_currentHealth, value))
                    return;
                
                _currentHealth = value;
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }

        public HealthModel(float maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float value)
        {
            if(value <= 0) 
                return;
            
            CurrentHealth = Mathf.Clamp(_currentHealth - value, 0f, MaxHealth);
        }

        public void Reset() =>  CurrentHealth = MaxHealth;

        public void RestoreHp() => CurrentHealth = MaxHealth;
    }
}
