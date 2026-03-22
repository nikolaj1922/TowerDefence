using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.Health;

namespace _Project.Scripts.Tower.Castle
{
    public class Castle : Tower, IDamagable
    {
        public event Action OnCastleDestroy;
        
        [SerializeField] private GameObject _castleModel;
        [SerializeField] private GameObject _castleDamagedModel;
        
        private HealthModel _healthModel;
        private bool _damagedModel = false;
        private bool _isGameOver = false;

        [Inject]
        public void Construct(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }

        private void Start()
        {
            _healthModel.OnHealthChanged += TakeDamage;
        }

        private void OnDisable() => _healthModel.OnHealthChanged -= TakeDamage;

        
        public void TakeDamage(float currentHealth)
        {
            if (currentHealth / _healthModel.MaxHealth > 0.5f)
                return;

            ChangeCastleModel();

            if (currentHealth <= 0 && !_isGameOver)
            {
                OnCastleDestroy?.Invoke();
                _isGameOver = true;
            }
        }

        private void ChangeCastleModel()
        {
            if (_damagedModel)
                return;
            
            _damagedModel = true;
            
            _castleDamagedModel.SetActive(true);
            _castleModel.SetActive(false);
            _weapon.gameObject.SetActive(false);
        }
    }
}