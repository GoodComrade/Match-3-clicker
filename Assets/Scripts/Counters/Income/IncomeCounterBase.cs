using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public abstract class IncomeCounterBase : MonoBehaviour
{
    [SerializeField] protected UpgradeToastUI ToastPrefab;
    [SerializeField] protected Transform ToastSpawnPoint;
    [SerializeField] protected List<UpgradeScriptableData> UpgradesData;

    protected List<IncomeUpgradeBase> Upgrades = new List<IncomeUpgradeBase>();
    protected PlayerStats Money;

    public IReadOnlyList<IncomeUpgradeBase> UpgradesReadonlyList => Upgrades;

    protected virtual void InitializeUpgrades()
    {
        Money = GetComponent<PlayerStats>();
    }
}


