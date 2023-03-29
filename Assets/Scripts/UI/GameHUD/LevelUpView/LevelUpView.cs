using UnityEngine;
using UnityEngine.UI;

public class LevelUpView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private LevelCounter _counter;
    [SerializeField] private Button _button;
    [SerializeField] private PayDayViewStat _rewardText;

    void Awake()
    {
        _counter.LevelUpViewOpened += OnOpenView;
        _button.onClick.AddListener(OnCloseView);
        gameObject.SetActive(false);
    }

    private void OnOpenView(float income)
    {
        Debug.Log("LEVELUP!");
        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        _rewardText.SetTextValue("IncomePerMonth:", income);

    }

    private void OnCloseView()
    {
        gameObject.SetActive(false);
    }
}
