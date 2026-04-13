using System;
using Zenject;

namespace _Project.Scripts.UI.CoinCounter
{
    public class CoinCounterController : IInitializable, IDisposable
    {
        private readonly CoinCounterModel _model;
        private readonly CoinCounterPanel _panelView;
        
        public CoinCounterController(CoinCounterModel model, CoinCounterPanel panelView)
        {
            _model = model;
            _panelView = panelView;
        }

        public void Initialize() => _model.OnCoinChanged += UpdateCoinView;
        
        public void Dispose() => _model.OnCoinChanged -= UpdateCoinView;

        private void UpdateCoinView(int coins) => _panelView.UpdateView(coins);
    }
}