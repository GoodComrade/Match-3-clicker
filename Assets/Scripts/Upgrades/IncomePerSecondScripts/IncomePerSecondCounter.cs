using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class IncomePerSecondCounter : IncomeCounterBase
{
    [SerializeField] private float _startPeriodicIncome = 0.1f;
    [SerializeField] private float _incomeDelay;

    private float _periodicIncome;

    public event UnityAction<float> GetIncome;

    private void Start()
    {
        _periodicIncome = _startPeriodicIncome;
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
        float totalMultiplier = 0;

        foreach (IncomePerSecondUpgrade upgrade in Upgrades)
            if (upgrade.IsBuyed)
                totalMultiplier += upgrade.GetTotalMultiplier();

        _periodicIncome = totalMultiplier > 0 ? _periodicIncome * totalMultiplier : _periodicIncome;
    }
}
