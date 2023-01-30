using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class IncomePerSecondCounter : IncomeCounterBase
{
    [SerializeField] private float _startPeriodicIncome = 0.1f;
    [SerializeField] private float _startIncomeDelay;

    private float _periodicIncome;
    private float _incomeDelay;

    public event UnityAction<float> GetIncome;

    private void Awake()
    {
        InitializeUpgrades();
    }

    protected override void InitializeUpgrades()
    {
        base.InitializeUpgrades();

        for (int i = 0; i < UpgradesData.Count; i++)
        {
            UpgradeToastUI upgrade = Instantiate(ToastPrefab, ToastSpawnPoint);
            IncomePerSecondUpgrade ips = upgrade.AddComponent<IncomePerSecondUpgrade>();
            ips.Init(Money, UpgradesData[i]);
            Upgrades.Add(ips);
        }

        _periodicIncome = _startPeriodicIncome;
        _incomeDelay = _startIncomeDelay;
        StartCoroutine(AppllyIncome());
    }

    private IEnumerator AppllyIncome()
    {
        while(true)
        {
            yield return new WaitForSeconds(_incomeDelay);

            if (Upgrades != null)
            {
                CalculateIncome();
            }

            GetIncome?.Invoke(_periodicIncome);
        }
    }

    private void CalculateIncome()
    {
        float currentIncome = _startPeriodicIncome;
        float currentDelay = _startIncomeDelay;
        float totalMultiplier = 0;
        float totalDelay = 0;

        foreach (IncomePerSecondUpgrade upgrade in Upgrades)
        {
            if (upgrade.IsBuyed)
            {
                switch (upgrade.Data.UpgradeType)
                {
                    case UpgradeType.IncomeAmount: totalMultiplier += upgrade.GetTotalMultiplier(); break;
                    case UpgradeType.IncomeDelay: totalDelay += upgrade.GetTotalMultiplier(); break;
                    default: break;
                }
            }
        }

        _periodicIncome = totalMultiplier > 0 ? currentIncome * totalMultiplier : _periodicIncome;
        _incomeDelay = totalDelay > 0 ? currentDelay - totalDelay : _incomeDelay;
    }
}
