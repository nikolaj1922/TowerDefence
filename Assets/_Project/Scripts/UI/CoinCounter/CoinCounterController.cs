using System;
using Zenject;

namespace _Project.Scripts.UI.CoinCounter
{
    public class CoinCounterController : IInitializable, IDisposable
    {
        private readonly CoinCounterModel _model;
        private readonly CoinCounterView _view;
        
        public CoinCounterController(CoinCounterModel model, CoinCounterView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize() => _model.OnCoinChanged += UpdateCoinView;
        
        public void Dispose() => _model.OnCoinChanged -= UpdateCoinView;

        private void UpdateCoinView(int coins) => _view.UpdateView(coins);
    }
}