using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DateCounter))]
[RequireComponent(typeof(PlayerStats))]
public class PayDayCounter : MonoBehaviour
{
    public event UnityAction<float, float, float, float> PayDayOpenView;

    private DateCounter _dateCounter;
    private PlayerStats _playerStats;
    private IncomePerTickCounter _incomePerTickCounter;
    private IncomePerMatchCounter _incomePerMatchCounter;

    private float _incomePerMonth;
    private float _outcomePerMonth;
    private float _totalIncomePerMonth;

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
        _playerStats.MonthOutcomeChanged += CalculateOutcomePerMonth;
    }

    private void OnDisable()
    {
        _dateCounter.PayDayHasCome -= OnPayDayHasCome;
        _playerStats.MonthIncomeChanged -= CalculateIncomePerMonth;
        _playerStats.MonthOutcomeChanged -= CalculateOutcomePerMonth;
    }

    private void CalculateIncomePerMonth(float value)
    {
        _incomePerMonth += value;
        _incomePerMonth = Constants.RoundValue(_incomePerMonth, Constants.HundredDivider);
    }

    private void CalculateOutcomePerMonth(float value)
    {
        _outcomePerMonth += value;
        _outcomePerMonth = Constants.RoundValue(_incomePerMonth, Constants.HundredDivider);
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
        _playerStats.PayTaxes(taxes);

        _totalIncomePerMonth = (_incomePerMonth - _outcomePerMonth) - taxes;
        PayDayOpenView.Invoke(_incomePerMonth, _outcomePerMonth, -taxes, _totalIncomePerMonth);

        _incomePerMonth = 0;
        _outcomePerMonth = 0;
        _totalIncomePerMonth = 0;
    }
}
