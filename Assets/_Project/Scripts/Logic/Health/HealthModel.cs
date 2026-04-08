using System;
using UnityEngine;

namespace _Project.Scripts.Logic.Health
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
                _currentHealth = value;
                OnHealthChanged?.Invoke(_currentHealth);
            }
        }

        public HealthModel(float maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        public void ChangeHealth(float value) => CurrentHealth = Mathf.Clamp(_currentHealth + value, 0f, MaxHealth);

        public void Reset() =>  CurrentHealth = MaxHealth;
    }
}
