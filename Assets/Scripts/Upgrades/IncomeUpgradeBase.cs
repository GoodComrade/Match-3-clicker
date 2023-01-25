using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class IncomeUpgradeBase : MonoBehaviour
{
    private bool _isBuyed;
    public bool IsBuyed => _isBuyed;

    public UpgradeScriptableData Data { get; private set; }
    
    protected float Multiplier;
    protected float UpgradeCost;
    protected PlayerMoney Money;

    public event UnityAction<float, float> ValuesUpdated;

    public void Init(PlayerMoney money, UpgradeScriptableData data)
    {
        Multiplier = data.BaseMultiplier;
        Money = money;
        UpgradeCost = data.BaseCost;
        Data = data;
        _isBuyed = false;
    }

    public virtual void IncreaseMultiplier()
    {
        Money.BuyUpgrade(UpgradeCost);

        if (IsBuyed)
            Multiplier += Data.BaseMultiplier / 0.5f;

        UpgradeCost += UpgradeCost * Multiplier;

        RoundValues();

        ValuesUpdated?.Invoke(UpgradeCost, Multiplier);

        if (_isBuyed == false)
            _isBuyed = true;
    }

    public float GetTotalMultiplier()
    {
        return Multiplier;
    }

    public bool TryBuyUpgrade()
    {
        return Money.TotalAmount >= UpgradeCost;
    }

    private void RoundValues()
    {
        UpgradeCost = Mathf.Round(UpgradeCost * 100f) / 100f;
        Multiplier = Mathf.Round(Multiplier * 100f) / 100f;
    }
}
