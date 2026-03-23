using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Logic.Health
{
    public class HealthController: IInitializable, IDisposable
    {
        private readonly HealthBarView _view;
        private readonly HealthModel _model;

        public HealthController(HealthModel model,  HealthBarView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(_model.CurrentHealth);
        }
        
        public void Dispose() => _model.OnHealthChanged -= UpdateHealthBar;
        
        private void UpdateHealthBar(float current)
        {
            Debug.Log($"View: {_view}");
            if (_view != null)
                _view.SetFill(current / _model.MaxHealth);
        }
    }
}