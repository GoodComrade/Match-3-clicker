using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateCounterView : MonoBehaviour
{
    [SerializeField] private DateCounter _counter;

    private TMP_Text _viewText;

    private void Awake() => _viewText = GetComponentInChildren<TMP_Text>();

    private void OnEnable() => _counter.DateChanged += OnDateChanged;

    private void OnDisable() => _counter.DateChanged -= OnDateChanged;

    private void OnDateChanged(int day, int month, int year)
    {
        _viewText.text = $"{day}.{month}.{year}";
    }
}
