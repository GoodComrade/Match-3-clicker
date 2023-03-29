using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class PlayerMoneyView : MonoBehaviour
{
    [SerializeField] private PlayerStats _money;
    [SerializeField] private TMP_Text _viewText;

    private void Awake() => _viewText = GetComponentInChildren<TMP_Text>();

    private void OnEnable() => _money.MoneyChanged += OnMoneyChanged;

    private void OnDisable() => _money.MoneyChanged -= OnMoneyChanged;

    private void OnMoneyChanged(float value)
    {
        _viewText.text = $"$ {value}";

        if (value >= Constants.ThousandDivider)
        {
            value /= Constants.ThousandDivider;
            value = Constants.RoundValue(value, Constants.TenDivider);
            _viewText.text = $"$ {value}K";
        }

    }
}
