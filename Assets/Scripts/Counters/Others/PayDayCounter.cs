using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PayDayCounter : MonoBehaviour
{
    public UnityAction<float, float> PayDayOpenView;

    private DateCounter _dateCounter;
    private PlayerStats _playerStats;
    private IncomePerTickCounter _incomePerTickCounter;
    private IncomePerMatchCounter _incomePerMatchCounter;

    private float _incomePerMonth;

    private void Awake()
    {
        _dateCounter = GetComponent<DateCounter>();
        _playerStats = GetComponent<PlayerStats>();
        _incomePerTickCounter = GetComponent<IncomePerTickCounter>();
        _incomePerMatchCounter = GetComponent<IncomePerMatchCounter>();
    }

    private void OnEnable()
    {
        _dateCounter.PayDayHasCome += OnPayDayHasCome;
        _playerStats.MonthIncomeChanged += CalculateIncomePerMonth;
    }

    private void OnDisable()
    {
        _dateCounter.PayDayHasCome -= OnPayDayHasCome;
        _playerStats.MonthIncomeChanged -= CalculateIncomePerMonth;
    }

    private void CalculateIncomePerMonth(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        _incomePerMonth += value;
    }

    private float CalculateTaxes()
    {
        float totalTaxes = 1;
        int totalByedUpgrades = 0;

        foreach (var upgrade in _incomePerTickCounter.UpgradesReadonlyList)
            if (upgrade.IsBuyed)
                totalByedUpgrades++;

        foreach (var upgrade in _incomePerMatchCounter.UpgradesReadonlyList)
            if (upgrade.IsBuyed)
                totalByedUpgrades++;

        return totalByedUpgrades == 0 ? totalTaxes : (totalTaxes * totalByedUpgrades) + totalTaxes;
    }

    private void OnPayDayHasCome()
    {
        float taxes = CalculateTaxes();
        PayDayOpenView?.Invoke(_incomePerMonth, -taxes);
        _playerStats.RemoveMoney(taxes);
        _incomePerMonth = 0;
    }
}
