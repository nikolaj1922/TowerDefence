using System;
using _Project.Scripts.Logic.Health;
using UnityEngine;

namespace _Project.Scripts.Logic.Health
{
    public class HealthModel
    {
        public event Action<float, float> OnHealthChanged;
        public event Action OnDied;

        private float _currentHealth;
        public float MaxHealth { get; private set; }

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                
                OnHealthChanged?.Invoke(_currentHealth, MaxHealth);
                
                if (value <= 0)
                    OnDied?.Invoke();
            }
        }

        public HealthModel(float maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        public void ChangeHealth(float value) => CurrentHealth = Mathf.Clamp(_currentHealth + value, 0f, MaxHealth);
    }
}