using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DateCounter : MonoBehaviour
{
    public event UnityAction<int, int, int> DateChanged;
    public event UnityAction PayDayHasCome;

    [SerializeField] private float _timeSpeed;

    private int _day;
    private int _month;
    private int _year;

    private int _firstDay = 1;
    private int _firstMonth = 1;
    private int _firstYear = 2001;


    private void Awake()
    {
        _day = _firstDay;
        _month = _firstMonth;
        _year = _firstYear;
    }

    private void Start()
    {
        DateChanged?.Invoke(_day, _month, _year);
        StartCoroutine(CountingDate());
    }

    private IEnumerator CountingDate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeSpeed);

            _day++;

            if (_day > Constants.FebMaxDay && _month == 2
                || _day > Constants.ShortMonthMaxDay && (_month == 4 || _month == 6 || _month == 9 || _month == 11)
                || _day > Constants.MaxDay)
                ResetDayAndMonth();

            if (_month > Constants.MaxMonth)
                ResetMonthAndYear();

            DateChanged?.Invoke(_day, _month, _year);
        }
    }

    private void ResetDayAndMonth()
    {
        _day = _firstDay;
        _month++;

        PayDayHasCome?.Invoke();
    }

    private void ResetMonthAndYear()
    {
        _month = _firstMonth;
        _year++;
    }
}
