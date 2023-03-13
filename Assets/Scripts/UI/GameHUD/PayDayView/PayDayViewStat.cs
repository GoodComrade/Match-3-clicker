using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PayDayViewStat : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetTextValue(string toastText, float value)
    {
        _text.text = $"{toastText} ${value}"; 
    }
}
