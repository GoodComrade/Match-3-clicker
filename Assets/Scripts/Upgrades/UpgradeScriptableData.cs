using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Upgrade Model", menuName = "Clicker Data Models/New upgrade data model")]
public class UpgradeScriptableData : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private float _baseCost;
    [SerializeField] private float _baseMultiplier;

    public float BaseCost => _baseCost;
    public float BaseMultiplier => _baseMultiplier;
    public string Title => _title;
}
