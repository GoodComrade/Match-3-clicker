using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelCounter : MonoBehaviour
{
    public event UnityAction<int> ExpirienceValueChanged;
    public event UnityAction<int, int> LevelValueChanged;
    public event UnityAction<float> LevelUpViewOpened;

    public int CurrentLevel => _currentLevel;
    public int MinExpirienceAmount => _minExpirienceAmount;
    public int MaxExpirienceAmount => _maxExpirienceAmount;
    public int MaxExpirienceAmountAtStart => _maxExpirienceAmountAtStart;

    [SerializeField] private int _maxExpirienceAmountAtStart;
    [SerializeField] private int _bonusReward;

    private MatchBoard _board;


    private int _currentLevel = 0;
    private int _currentExpirienceAmount = 0;
    private int _minExpirienceAmount = 0;
    private int _maxExpirienceAmount;

    private void Awake()
    {
        _board = GetComponent<MatchBoard>();
        _board.SendExpirience += IncreaseCurrentExpirienceAmount;
        _maxExpirienceAmount = _maxExpirienceAmountAtStart;
    }

    private void OnDisable()
    {
        _board.SendExpirience -= IncreaseCurrentExpirienceAmount;
    }

    private void IncreaseCurrentExpirienceAmount(int value)
    {
        _currentExpirienceAmount += value;
        ExpirienceValueChanged?.Invoke(value);

        if (_currentExpirienceAmount >= _maxExpirienceAmount)
            IncreaseCurrentLevel();
    }

    private void SetMaxExpirienceAmount(int value)
    {
        _maxExpirienceAmount = value;
    }

    public void IncreaseCurrentLevel()
    {
        LevelUpViewOpened?.Invoke(_bonusReward);

        _currentLevel++;

        int multiplier = MaxExpirienceAmount / 20;
        int newMaxExpValue = _maxExpirienceAmount * multiplier;
        SetMaxExpirienceAmount(newMaxExpValue);

        LevelValueChanged?.Invoke(_currentLevel, _maxExpirienceAmount);
        

    }
}
