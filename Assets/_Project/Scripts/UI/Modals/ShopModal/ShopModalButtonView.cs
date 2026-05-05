using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopModalButtonView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;

    private Action _onClick;
    
    public void Init(string title, string price, Action onClick)
    {
        _buyButton.onClick.RemoveListener(OnBuyClick); 
        
        _onClick = onClick;
        
        _buyButton.onClick.AddListener(OnBuyClick);
        _titleText.text = title;
        _priceText.text = price;
    }

    private void OnDestroy() => _buyButton.onClick.RemoveListener(OnBuyClick);

    private void OnBuyClick() => _onClick?.Invoke();
}
