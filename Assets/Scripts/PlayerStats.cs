using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public event UnityAction<float> MoneyChanged;
    public event UnityAction<float> MonthIncomeChanged;

    private MatchBoard _board;
    public float TotalMoney { get; private set; }

    private IncomePerTickCounter _counter;

    private void Awake()
    {
        _counter = GetComponent<IncomePerTickCounter>();
        _board = GetComponent<MatchBoard>();
    }

    private void Start() => MoneyChanged?.Invoke(TotalMoney);

    private void OnEnable()
    {
        _counter.IncomeEarned += AddMoney;
        _board.SendReward += AddMoney;
    }

    private void OnDisable()
    {
        _counter.IncomeEarned -= AddMoney;
        _board.SendReward -= AddMoney;
    }

    private void AddMoney(float money)
    {
        TotalMoney += money;
        TotalMoney = Mathf.Round(TotalMoney * 10f) / 10f;

        MoneyChanged?.Invoke(TotalMoney);
        MonthIncomeChanged?.Invoke(money);
    }

    public void RemoveMoney(float cost)
    {
        TotalMoney += -cost;
        TotalMoney = Mathf.Round(TotalMoney * 10f) / 10f;

        MoneyChanged?.Invoke(TotalMoney);
        MonthIncomeChanged?.Invoke(-cost);
    }
}
