using System;
using UnityEngine;
using _Project.Scripts.Logic.Health;

namespace _Project.Scripts.Tower
{
    public class CastleController : HealthController
    {
        public event Action OnCastleDestroy;
        
        [SerializeField] private GameObject _castleModel;
        [SerializeField] private GameObject _castleDamagedModel;
        [SerializeField] private GameObject _castleWeapon;
        
        private bool _damagedModel = false;
        private bool _isGameOver = false;

        private void OnDisable() => HealthModel.OnHealthChanged -= TakeDamage;
        
        public override void InitHealth(float health)
        {
            base.InitHealth(health);
            HealthModel.OnHealthChanged += TakeDamage;
        }

        private void TakeDamage(float currentHealth, float maxHealth)
        {
            if (currentHealth / maxHealth > 0.5f)
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
            _castleModel.SetActive(false);
            _castleWeapon.SetActive(false);
            _castleDamagedModel.SetActive(true);
        }
    }
}