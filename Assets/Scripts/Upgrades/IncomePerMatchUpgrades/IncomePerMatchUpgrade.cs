using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IncomePerMatchUpgrade : IncomeUpgradeBase
{
    //TODO: add here increase tile income logic
    // It shoud be applied by type of upgrade 
    /* types of upgrade: 
     * 1. certain tile upgrade type
     * 2. match strick upgrade type
     * 3. match length upgrade type
     * 4. opener new tile upgrade type
     */

    public event UnityAction<float, UpgradeType> ValueIncreased;

    public override void IncreaseMultiplier()
    {
        base.IncreaseMultiplier();

        if (IsBuyed)
        {
            ValueIncreased.Invoke(Multiplier, Data.UpgradeType);
        }
            
    }
}
