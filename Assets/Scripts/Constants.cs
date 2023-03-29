using UnityEngine;

public static class Constants
{
    public const int TenDivider = 10;
    public const int HundredDivider = 100;
    public const int ThousandDivider = 1000;

    public const int MaxDay = 31;
    public const int FebMaxDay = 28;
    public const int ShortMonthMaxDay = 30;
    public const int MaxMonth = 12;

    public static float RoundValue(float value, int divider)
    {
        float roundedValue = Mathf.Round(value * divider) / divider;
        return roundedValue;
    }
}
