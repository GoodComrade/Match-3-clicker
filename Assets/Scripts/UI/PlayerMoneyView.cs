using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyView : MonoBehaviour
{
    [SerializeField] private PlayerMoney _money;
    [SerializeField] private TMP_Text _viewText;

    private void Awake()
    {
        _money.MoneyChanged += OnMoneyChanged;
    }

    private void Start()
    {
        _viewText.text = $"$ {_money.TotalAmount}";
    }

    private void OnDisable()
    {
        _money.MoneyChanged -= OnMoneyChanged;
    }

    private void OnMoneyChanged(float value)
    {
        _viewText.text = $"$ {value}";

        if (value >= Constants.ThousandDivider)
        {
            value /= Constants.ThousandDivider;
            value = value = Mathf.Round(value * Constants.TenDivider) / Constants.TenDivider; ;
            _viewText.text = $"$ {value}K";
        }
        
    }
}
