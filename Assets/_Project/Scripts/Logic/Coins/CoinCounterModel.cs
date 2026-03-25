using System;

namespace _Project.Scripts.Logic.Coins
{
    public class CoinCounterModel
    {
        public event Action<int> OnCoinChanged; 
        
        public int Coins { get; private set; }

        public void AddCoins(int coins)
        {
            Coins += coins;
            OnCoinChanged?.Invoke(Coins);
        }

        public void RemoveCoins(int coins)
        {
            Coins -= coins;
            OnCoinChanged?.Invoke(Coins);
        }
    }
}