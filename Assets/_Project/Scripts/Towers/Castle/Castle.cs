using System;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Logic.Health;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Towers.Castle
{
    public class Castle : Tower, IDamagable
    {
        public event Action OnCastleDestroy;
        
        [SerializeField] private GameObject _castleModel;
        [SerializeField] private GameObject _castleDamagedModel;
        [SerializeField] private ParticleSystem _onCollapseEffect;
        private StateMachine _stateMachine;

        public HealthModel HealthModel { get; private set; }
        
        [Inject]
        public void Construct([Inject(Id = GameConstants.CASTLE_HEALTH_MODEL_INJECT_ID)] HealthModel healthModel) => HealthModel = healthModel;
        
        private void Update() => _stateMachine?.Update();
        
        public void SetStateMachine(StateMachine stateMachine) => _stateMachine = stateMachine;
        
        public void TakeDamage(float damage)
        {
            HealthModel.ChangeHealth(-damage);
            
            if (HealthModel.CurrentHealth > 0)
                return;
            
            OnCastleDestroy?.Invoke();
        }

        public void Collapse()
        {
            Instantiate(_onCollapseEffect, transform.position, Quaternion.identity);
            
            _castleDamagedModel.SetActive(true);
            _castleModel.SetActive(false);
            _weapon.gameObject.SetActive(false);
        }
    }
}