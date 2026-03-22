using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.UI.HealthBar;
using _Project.Scripts.Tower.Weapon;

namespace _Project.Scripts.Tower.Castle
{
    public class CastleController : MonoBehaviour, IDamagable
    {
        public event Action OnCastleDestroy;
        
        [SerializeField] private GameObject _castleModel;
        [SerializeField] private GameObject _castleDamagedModel;

        private HealthModel _healthModel;
        private Weapon1 _weapon;
        private bool _damagedModel = false;
        private bool _isGameOver = false;

        [Inject]
        public void Construct(HealthModel healthModel, Weapon1 weapon)
        {
            _healthModel = healthModel;
            _weapon = weapon;
        }

        private void Start()
        {
            _healthModel.OnHealthChanged += TakeDamage;
        }

        private void Update()
        {
            _weapon.Tick(Time.deltaTime);
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
            // Weapon.gameObject.SetActive(false);
        }
    }
}