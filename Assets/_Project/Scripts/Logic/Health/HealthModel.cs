using System;
using UnityEngine;

namespace _Project.Scripts.Logic.Health
{
    public class HealthModel: IDisposable
    {
        public event Action<float> OnHealthChanged;
        public event Action OnDeath;

        private float _currentHealth;
        public float MaxHealth { get; }

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                OnHealthChanged?.Invoke(_currentHealth);

                if (value <= 0)
                    OnDeath?.Invoke();
            }
        }

        public HealthModel(float maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        public void ChangeHealth(float value) => CurrentHealth = Mathf.Clamp(_currentHealth + value, 0f, MaxHealth);

        public void Reset() =>  CurrentHealth = MaxHealth;

        public void Dispose()
        {
            OnDeath = null;
            OnHealthChanged = null;
        }
    }
}
