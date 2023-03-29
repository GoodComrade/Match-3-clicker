using UnityEngine.Events;

public class IncomePerMatchUpgrade : IncomeUpgradeBase
{
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
