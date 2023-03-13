using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayDayView : MonoBehaviour
{
    [SerializeField] private PayDayCounter _counter;
    [SerializeField] private Button _button;
    [SerializeField] private PayDayViewStat _incomeToast;
    [SerializeField] private PayDayViewStat _taxToast;

    void Awake()
    {
        _counter.PayDayOpenView += OnOpenView;
        _button.onClick.AddListener(OnCloseView);
        gameObject.SetActive(false);
    }
    
    private void OnOpenView(float income, float taxes)
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);

        _incomeToast.SetTextValue(income);
        _taxToast.SetTextValue(taxes);
    }
    private void OnCloseView()
    {
        gameObject.SetActive(false);
    }
}
