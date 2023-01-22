using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMoney : MonoBehaviour
{
    public float TotalAmount { get; private set; }

    private IncomePerSecondCounter _counter;

    public event UnityAction<float> MoneyChanged;

    private void Awake()
    {
        _counter = GetComponent<IncomePerSecondCounter>();
    }

    private void OnEnable()
    {
        _counter.GetIncome += AddMoney;
    }

    private void OnDisable()
    {
        _counter.GetIncome -= AddMoney;
    }

    private void AddMoney(float money)
    {
        TotalAmount += money;
        TotalAmount = Mathf.Round(TotalAmount * 10f) / 10f;
    }

    public void BuyUpgrade(float cost)
    {
        TotalAmount -= cost;
    }
}
