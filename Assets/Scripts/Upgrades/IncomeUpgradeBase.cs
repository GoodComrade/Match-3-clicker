using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class IncomeUpgradeBase : MonoBehaviour
{
    public event UnityAction<float, float> ValuesUpdated;
    public event UnityAction<UpgradeType> UpgradeBuyed;

    public bool IsBuyed => _isBuyed;

    public UpgradeScriptableData Data { get; private set; }

    private bool _isBuyed;
    private AudioSource _audioSource;

    protected float Multiplier;
    protected float UpgradeCost;
    protected PlayerStats Money;

    public void Init(PlayerStats money, UpgradeScriptableData data)
    {
        Multiplier = data.BaseMultiplier;
        Money = money;
        UpgradeCost = data.BaseCost;
        Data = data;
        _isBuyed = false;


        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public virtual void IncreaseMultiplier()
    {
        _audioSource?.Play();
        Money.RemoveMoney(UpgradeCost);

        if (IsBuyed)
        {
            UpgradeCost += UpgradeCost * Multiplier;
            Multiplier += Data.BaseMultiplier / 0.5f;
        }

        RoundValues();

        ValuesUpdated?.Invoke(UpgradeCost, Multiplier);

        if (_isBuyed == false)
            BuyUpgrade();
    }

    public float GetTotalMultiplier()
    {
        return Multiplier;
    }

    public bool TryBuyUpgrade()
    {
        return Money.TotalMoney >= UpgradeCost;
    }

    private void BuyUpgrade()
    {
        _isBuyed = true;
        UpgradeBuyed?.Invoke(Data.UpgradeType);
    }
    private void RoundValues()
    {
        UpgradeCost = Mathf.Round(UpgradeCost * 100f) / 100f;
        Multiplier = Mathf.Round(Multiplier * 100f) / 100f;
    }
}
