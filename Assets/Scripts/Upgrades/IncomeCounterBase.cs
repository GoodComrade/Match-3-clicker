using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IncomeCounterBase : MonoBehaviour
{
    [SerializeField] protected UpgradeToastUI ToastPrefab;
    [SerializeField] protected Transform ToastSpawnPoint;
    [SerializeField] protected List<UpgradeScriptableData> UpgradesData;

    protected readonly List<IncomeUpgradeBase> Upgrades = new List<IncomeUpgradeBase>();
    protected PlayerMoney Money;

    protected virtual void InitializeUpgrades()
    {
        Money = GetComponent<PlayerMoney>();
    }
}


