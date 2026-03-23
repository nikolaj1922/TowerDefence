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
        public void Construct(HealthModel healthModel) =>  _healthModel = healthModel;
        
        public void TakeDamage(float damage)
        {
            _healthModel.ChangeHealth(-damage);
            
            if (_healthModel.CurrentHealth / _healthModel.MaxHealth > 0.5f)
                return;

            ChangeCastleModel();

            if (_healthModel.CurrentHealth <= 0 && !_isGameOver)
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