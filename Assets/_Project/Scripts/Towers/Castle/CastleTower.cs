using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.UI.HealthBar;

namespace _Project.Scripts.Towers.Castle
{
    [RequireComponent(typeof(Tower))]
    public class CastleTower : MonoBehaviour, IDamagable, ICastleTower
    {
        public event Action OnCastleDestroy;
        public event Action<float> OnCastleDamaged;
        
        [SerializeField] private GameObject _castleModel;
        [SerializeField] private GameObject _castleDamagedModel;
        [SerializeField] private ParticleSystem _onCollapseEffect;
        private Tower _tower;
        private StateMachine _stateMachine;

        public HealthModel HealthModel { get; private set; }
        
        [Inject]
        public void Construct(HealthModel healthModel) => HealthModel = healthModel;
        
        private void Awake() => _tower = GetComponent<Tower>();
        
        private void Update() => _stateMachine?.Update();
        
        public void SetStateMachine(StateMachine stateMachine) => _stateMachine = stateMachine;

        public void RestoreHp() => HealthModel.RestoreHp();
        
        public void TakeDamage(float damage)
        {
            HealthModel.TakeDamage(damage);

            if (HealthModel.CurrentHealth > 0)
            {
                OnCastleDamaged?.Invoke(HealthModel.CurrentHealth);
                return;
            }
            
            OnCastleDestroy?.Invoke();
        }

        public void BreakWeapon()
        {
            Instantiate(_onCollapseEffect, transform.position, Quaternion.identity);
            
            _castleDamagedModel.SetActive(true);
            _castleModel.SetActive(false);
            _tower.Weapon.gameObject.SetActive(false);
        }

        public void RestoreWeapon()
        {
            _castleDamagedModel.SetActive(false);
            _castleModel.SetActive(true);
            _tower.Weapon.gameObject.SetActive(true);
        }
    }
}