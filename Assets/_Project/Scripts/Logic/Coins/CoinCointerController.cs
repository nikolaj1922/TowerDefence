using System;
using Zenject;
using _Project.Scripts.UI.CoinCounter;

namespace _Project.Scripts.Logic.Coins
{
    public class CoinCounterController : IInitializable, IDisposable
    {
        private readonly CoinCounterModel _model;
        private readonly  CoinCounterPanel _panel;
        
        public CoinCounterController(CoinCounterModel model, CoinCounterPanel panel)
        {
            _model = model;
            _panel = panel;
        }

        public void Initialize() => _model.OnCoinChanged += UpdateCoinView;
        
        public void Dispose() => _model.OnCoinChanged -= UpdateCoinView;

        private void UpdateCoinView(int coins) => _panel.UpdateView(coins);
    }
}