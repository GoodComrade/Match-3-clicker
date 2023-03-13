using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayDayView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private PayDayCounter _counter;
    [SerializeField] private Button _button;
    [SerializeField] private PayDayViewStat _incomeToast;
    [SerializeField] private PayDayViewStat _outcomeToast;
    [SerializeField] private PayDayViewStat _taxToast;
    [SerializeField] private PayDayViewStat _totalIncomeToast;

    [Header("Sound")]
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _looseClip;
    private AudioSource _audioSource;
    

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _counter.PayDayOpenView += OnOpenView;
        _button.onClick.AddListener(OnCloseView);
        gameObject.SetActive(false);
    }
    
    private void OnOpenView(float income, float outcome, float taxes, float totalIncome)
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);

        _incomeToast.SetTextValue("IncomePerMonth:", income);
        _outcomeToast.SetTextValue("IncomePerMonth:", - outcome);
        _taxToast.SetTextValue("Taxes:", taxes);
        _totalIncomeToast.SetTextValue("TotalIncome:", totalIncome);

        if(totalIncome <= 0)
        {
            _audioSource.clip = _looseClip;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = _winClip;
            _audioSource.Play();
        }

    }
    private void OnCloseView()
    {
        gameObject.SetActive(false);
    }
}
