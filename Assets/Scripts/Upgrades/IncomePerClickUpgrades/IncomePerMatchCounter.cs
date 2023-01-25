using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IncomePerMatchCounter : IncomeCounterBase
{
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
            IncomePerMatchUpgrade ips = upgrade.AddComponent<IncomePerMatchUpgrade>();
            ips.Init(Money, UpgradesData[i]);
            Upgrades.Add(ips);
        }
    }

    //TODO: make match3 mechanic


    protected override void CalculateIncome()
    {
        float totalMultiplier = 0;

        foreach (IncomePerSecondUpgrade upgrade in Upgrades)
            if (upgrade.IsBuyed)
                totalMultiplier += upgrade.GetTotalMultiplier();

        //TODO: apply total incomePerMatch multiplier to IncomePerMatch value
    }
}
