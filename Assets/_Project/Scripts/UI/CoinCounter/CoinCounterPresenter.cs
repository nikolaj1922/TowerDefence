using System;
using Zenject;

namespace _Project.Scripts.UI.CoinCounter
{
    public class CoinCounterPresenter : IInitializable, IDisposable
    {
        private readonly CoinCounterModel _model;
        private readonly CoinCounterView _view;
        
        public CoinCounterPresenter(CoinCounterModel model, CoinCounterView view)
        {
            _model = model;
            _view = view;
        }

        public void Initialize()
        {
            _model.OnCoinChanged += OnCoinsChanged;
            _view.SetCoins(_model.Coins);
        }
        
        public void Dispose() => _model.OnCoinChanged -= OnCoinsChanged;

        private void OnCoinsChanged(int coins) => _view.SetCoins(coins);
    }
}