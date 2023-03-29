using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static readonly int TenDivider = 10;
    public static readonly int HundredDivider = 100;
    public static readonly int ThousandDivider = 1000;

    public static readonly int MaxDay = 31;
    public static readonly int FebMaxDay = 28;
    public static readonly int ShortMonthMaxDay = 30;
    public static readonly int MaxMonth = 12;

    public static float RoundValue(float value, int divider)
    {
        float roundedValue = Mathf.Round(value * divider) / divider;
        return roundedValue;
    }
}
