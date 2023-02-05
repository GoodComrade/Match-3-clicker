using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IncomePerMatchCounter : IncomeCounterBase
{
    private MatchBoard _matchBoard;

    private void Awake()
    {
        _matchBoard = GetComponent<MatchBoard>();
        InitializeUpgrades();
    }

    private void OnDisable()
    {
        foreach(IncomePerMatchUpgrade upgrade in Upgrades)
        {
            upgrade.ValueIncreased -= ApplyMultiplier;
        }
    }

    protected override void InitializeUpgrades()
    {
        base.InitializeUpgrades();

        for (int i = 0; i < UpgradesData.Count; i++)
        {
            UpgradeToastUI upgrade = Instantiate(ToastPrefab, ToastSpawnPoint);
            IncomePerMatchUpgrade ipm = upgrade.AddComponent<IncomePerMatchUpgrade>();
            ipm.Init(Money, UpgradesData[i]);
            ipm.ValueIncreased += ApplyMultiplier;
            Upgrades.Add(ipm);
        }
    }


    private void ApplyMultiplier(float value, UpgradeType upgradeType)
    {
        for(int i = 0; i < Upgrades.Count; i++)
        {
            if ((int)_matchBoard.TileDatas[i].TileType == (int)upgradeType)
                _matchBoard.TileDatas[i].SetReward(value);
        }
    }
}
