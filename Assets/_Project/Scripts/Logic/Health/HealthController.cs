using System;
using Zenject;
using _Project.Scripts.UI.HealthBar;

namespace _Project.Scripts.Logic.Health
{
    public class HealthController: IInitializable, IDisposable
    {
        private readonly HealthBarView _view;
        private readonly HealthModel _model;

        public HealthController(HealthBarView view, HealthModel model)
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
            if (_view != null)
                _view.SetFill(current / _model.MaxHealth);
        }
    }
}