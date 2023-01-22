using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomePerMatchCounter : IncomeCounterBase
{
    //TODO: make match3 mechanic

    private void CalculateIncome()
    {
        float totalMultiplier = 0;

        foreach (IncomePerSecondUpgrade upgrade in Upgrades)
            if (upgrade.IsBuyed)
                totalMultiplier += upgrade.GetTotalMultiplier();

        //TODO: apply total incomePerMatch multiplier to IncomePerMatch value
    }
}
