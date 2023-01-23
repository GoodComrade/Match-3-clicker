using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class IncomeUpgradeBase : MonoBehaviour
{
    private UpgradeScriptableData _data;

    private bool _isBuyed;

    public UpgradeScriptableData Data => _data;
    public bool IsBuyed => _isBuyed;
    public float Multiplier { get; private set; }
    public float UpgradeCost { get; private set; }
    public PlayerMoney Money { get; private set; }

    public event UnityAction<float, float> ValuesUpdated;

    public void Init(PlayerMoney money, UpgradeScriptableData data)
    {
        Multiplier = data.BaseMultiplier;
        Money = money;
        UpgradeCost = data.BaseCost;
        _data = data;
        _isBuyed = false;
    }

    public void IncreaseMultiplier()
    {
        Money.BuyUpgrade(UpgradeCost);

        if (_isBuyed)
            Multiplier *= _data.BaseMultiplier;

        UpgradeCost *= Multiplier;
        RoundValues();

        ValuesUpdated?.Invoke(UpgradeCost, Multiplier);

        if (_isBuyed == false)
            _isBuyed = true;
    }

    public float GetTotalMultiplier()
    {
        return Multiplier;
    }

    public double GetTotalMoney()
    {
        return Money.TotalAmount;
    }

    private void RoundValues()
    {
        UpgradeCost = Mathf.Round(UpgradeCost * 100f) / 100f;
        Multiplier = Mathf.Round(Multiplier * 100f) / 100f;
    }
}
