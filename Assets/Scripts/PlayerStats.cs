using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public event UnityAction<float> MoneyChanged;
    public event UnityAction<float> MonthIncomeChanged;
    public event UnityAction<float> MonthOutcomeChanged;

    private MatchBoard _board;
    private LevelCounter _levelCounter;
    public float TotalMoney { get; private set; }

    private IncomePerTickCounter _counter;

    private void Awake()
    {
        _counter = GetComponent<IncomePerTickCounter>();
        _board = GetComponent<MatchBoard>();
        _levelCounter = GetComponent<LevelCounter>();
    }

    private void Start() => MoneyChanged?.Invoke(TotalMoney);

    private void OnEnable()
    {
        _counter.IncomeEarned += AddMoney;
        _board.SendReward += AddMoney;
        _levelCounter.LevelUpViewOpened += AddMoney;
    }

    private void OnDisable()
    {
        _counter.IncomeEarned -= AddMoney;
        _board.SendReward -= AddMoney;
        _levelCounter.LevelUpViewOpened -= AddMoney;
    }

    private void AddMoney(float money)
    {
        TotalMoney += money;
        TotalMoney = Constants.RoundValue(TotalMoney, Constants.TenDivider);

        MoneyChanged?.Invoke(TotalMoney);
        MonthIncomeChanged?.Invoke(money);
    }

    public void RemoveMoney(float cost)
    {
        TotalMoney -= cost;
        TotalMoney = Constants.RoundValue(TotalMoney, Constants.TenDivider);

        MoneyChanged?.Invoke(TotalMoney);
        MonthOutcomeChanged?.Invoke(cost);
    }

    public void PayTaxes(float tax)
    {
        TotalMoney -= tax;
        TotalMoney = Constants.RoundValue(TotalMoney, Constants.TenDivider);

        MoneyChanged?.Invoke(TotalMoney);
    }
}
