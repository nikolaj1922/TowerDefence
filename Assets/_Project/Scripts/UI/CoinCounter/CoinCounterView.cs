using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.CoinCounter
{
    public class CoinCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        
        public void SetCoins(int coins) => _coinText.text = coins.ToString();
    }
}