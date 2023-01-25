using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeToastUI : MonoBehaviour
{
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TMP_Text _multiplierText;
    [SerializeField] private TMP_Text _titleText;

    private IncomeUpgradeBase _toastLogic;
    private TMP_Text _buttonText;
    private CanvasGroup _visual;

    private void Start()
    {
        _toastLogic = GetComponent<IncomeUpgradeBase>();
        _buttonText = _upgradeButton.GetComponentInChildren<TMP_Text>();
        _visual = GetComponentInChildren<CanvasGroup>();

        if(_toastLogic != null)
        {
            _toastLogic.ValuesUpdated += OnValuesChanged;
            _upgradeButton.onClick.AddListener(_toastLogic.IncreaseMultiplier);

            _titleText.text = _toastLogic.Data.Title;
            _multiplierText.text = _toastLogic.Data.BaseMultiplier.ToString();
            _buttonText.text = _toastLogic.Data.BaseCost.ToString();
        }
    }

    private void Update()
    {
        if (_toastLogic.TryBuyUpgrade())
            _upgradeButton.interactable = true;
        else
            _upgradeButton.interactable = false;

        if(_visual.alpha < 1f)
            _visual.alpha = 1f;

        if (_toastLogic.IsBuyed == true)
            return;

        _visual.alpha = 0.5f;
    }

    /*private void OnDisable()
    {
        _toastLogic.ValuesUpdated -= OnValuesChanged;
    }*/

    private void OnValuesChanged(float cost, float multiplier)
    {
        _multiplierText.text = multiplier.ToString();
        _buttonText.text = cost.ToString();
    }
}
