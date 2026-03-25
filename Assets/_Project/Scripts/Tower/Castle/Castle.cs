using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Tower.Castle
{
    public class Castle : Tower, IDamagable
    {
        public event Action OnCastleDestroy;
        
        [SerializeField] private GameObject _castleModel;
        [SerializeField] private GameObject _castleDamagedModel;

        public HealthModel HealthModel { get; private set; }

        private StateMachine _stateMachine;

        [Inject]
        public void Construct([Inject(Id = "CastleHealthModel")] HealthModel healthModel) => HealthModel = healthModel;
        
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
            _castleDamagedModel.SetActive(true);
            _castleModel.SetActive(false);
            _weapon.gameObject.SetActive(false);
        }
    }
}