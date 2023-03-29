using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCounterView : MonoBehaviour
{
    [SerializeField] private LevelCounter _counter;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Slider _slider;

    private Coroutine _updateValue;

    private void Start()
    {
        InitValues();
    }

    private void OnEnable()
    {
        _counter.ExpirienceValueChanged += OnExpirienceValueChanged;
        _counter.LevelValueChanged += OnLevelValueChanged;
    }

    private void OnDisable()
    {
        _counter.ExpirienceValueChanged -= OnExpirienceValueChanged;
        _counter.LevelValueChanged -= OnLevelValueChanged;
    }

    private void InitValues()
    {
        _slider.maxValue = _counter.MaxExpirienceAmount;
        _slider.value = _counter.MinExpirienceAmount;
        _text.text = _counter.CurrentLevel.ToString();
    }

    private void OnLevelValueChanged(int levelValue, int maxExpirienceValue)
    {
        _slider.maxValue = maxExpirienceValue;
        _text.text = levelValue.ToString();
    }

    private void OnExpirienceValueChanged(float value)
    {
        _slider.value += value;

        if (_slider.value == _slider.maxValue)
            _slider.value = _counter.MinExpirienceAmount;

    }

}
