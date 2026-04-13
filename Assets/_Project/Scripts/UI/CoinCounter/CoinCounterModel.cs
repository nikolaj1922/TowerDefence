using System;

namespace _Project.Scripts.UI.CoinCounter
{
    public class CoinCounterModel
    {
        public event Action<int> OnCoinChanged;

        private int _coins;
        public int Coins
        {
            get => _coins;
            private set
            {
                _coins = value;
                OnCoinChanged?.Invoke(_coins);
            }
        }
        
        public void UpdateSubscribers() => OnCoinChanged?.Invoke(_coins);

        public void AddCoins(int coins) => Coins += coins;
        
        public void RemoveCoins(int coins) => Coins -= coins;
    }
}