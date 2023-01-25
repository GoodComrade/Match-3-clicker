using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Upgrade Model", menuName = "Clicker Data Models/New upgrade data model")]
public class UpgradeScriptableData : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private float _baseCost;
    [SerializeField] private float _baseMultiplier;
    [SerializeField] private UpgradeType _upgradeType;

    public float BaseCost => _baseCost;
    public float BaseMultiplier => _baseMultiplier;
    public string Title => _title;
    public UpgradeType UpgradeType => _upgradeType;
}


public enum UpgradeType
{
    Red,
    Green,
    Blue,
    IncomeDelay,
    IncomeAmount
}
