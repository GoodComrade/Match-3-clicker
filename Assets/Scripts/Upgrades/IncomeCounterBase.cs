using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IncomeCounterBase : MonoBehaviour
{
    [SerializeField] protected UpgradeToastUI ToastPrefab;
    [SerializeField] protected Transform ToastSpawnPoint;
    [SerializeField] protected List<UpgradeScriptableData> UpgradesData;

    protected readonly List<IncomePerSecondUpgrade> Upgrades = new List<IncomePerSecondUpgrade>();
    protected PlayerMoney Money;

    private void Awake()
    {
        Money = GetComponent<PlayerMoney>();

        InitializeUpgrades();
    }

    public List<IncomePerSecondUpgrade> GetUpgradesList()
    {
        return Upgrades;
    }

    protected void InitializeUpgrades()
    {
        for (int i = 0; i < UpgradesData.Count; i++)
        {
            UpgradeToastUI upgrade = Instantiate(ToastPrefab, ToastSpawnPoint);
            IncomePerSecondUpgrade ips = upgrade.AddComponent<IncomePerSecondUpgrade>();
            ips.Init(Money, UpgradesData[i]);
            Upgrades.Add(ips);
        }
    }
}


