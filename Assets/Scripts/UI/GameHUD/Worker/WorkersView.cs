using System.Collections.Generic;
using UnityEngine;

public class WorkersView : MonoBehaviour
{
    [SerializeField]
    private IncomePerMatchCounter _ipmCounter;
    [SerializeField]
    private List<WorkerView> _workersList;


    private void Start()
    {
        SubscribeToUpgrades();
        DisableWorkers();
    }

    private void OnDisable()
    {
        UnsubscribeFromUpgrades();
    }

    private void SubscribeToUpgrades()
    {
        foreach (IncomeUpgradeBase upgrade in _ipmCounter.UpgradesReadonlyList)
        {
            if (upgrade.Data.UpgradeType != UpgradeType.IncomeDelay && upgrade.Data.UpgradeType != UpgradeType.IncomeAmount)
                upgrade.UpgradeBuyed += ActivateWorkerView;
        }
    }

    private void UnsubscribeFromUpgrades()
    {
        foreach (IncomeUpgradeBase upgrade in _ipmCounter.UpgradesReadonlyList)
        {
            if (upgrade.Data.UpgradeType != UpgradeType.IncomeDelay && upgrade.Data.UpgradeType != UpgradeType.IncomeAmount)
                upgrade.UpgradeBuyed -= ActivateWorkerView;
        }
    }

    private void ActivateWorkerView(UpgradeType upgradeType)
    {
        GameObject workerViewObj = null;

        switch (upgradeType)
        {
            case UpgradeType.RedTileUpgrade:
                workerViewObj = GetWorkerView(TileType.Red).gameObject;
                break;

            case UpgradeType.GreenTileUpgrade:
                workerViewObj = GetWorkerView(TileType.Green).gameObject;
                break;

            case UpgradeType.BlueTileUpgrade:
                workerViewObj = GetWorkerView(TileType.Blue).gameObject;
                break;

            case UpgradeType.YellowTileUpgrade:
                workerViewObj = GetWorkerView(TileType.Yellow).gameObject;
                break;

            case UpgradeType.PurpleTileUpgrade:
                workerViewObj = GetWorkerView(TileType.Purple).gameObject;
                break;
        }

        if (workerViewObj != null)
            workerViewObj.SetActive(true);
    }

    private WorkerView GetWorkerView(TileType workerType)
    {
        foreach (WorkerView worker in _workersList)
            if (worker.WorkerType == workerType)
                return worker;

        return null;
    }

    private void DisableWorkers()
    {
        foreach (WorkerView worker in _workersList)
            worker.gameObject.SetActive(false);
    }
}
