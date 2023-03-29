using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MatchBoard))]
[RequireComponent(typeof(XpManager))]
public class LevelCounter : MonoBehaviour
{
    public const string EXPIRIENCE_PER_LEVEL = "ExpiriencePerLevel";
    public event UnityAction<float> ExpirienceValueChanged;
    public event UnityAction<int, int> LevelValueChanged;
    public event UnityAction<float> LevelUpViewOpened;

    public int CurrentLevel => _currentLevel;
    public int MinExpirienceAmount => _minExpirienceAmount;
    public int MaxExpirienceAmount => _maxExpirienceAmount;

    [SerializeField] private int _bonusReward;

    private MatchBoard _board;
    private XpManager _xpManager;

    private int _currentLevel = 1;
    private float _currentExpirienceAmount = 0;
    private int _minExpirienceAmount = 0;
    private int _maxExpirienceAmount;

    private void Awake()
    {
        _board = GetComponent<MatchBoard>();
        _xpManager = GetComponent<XpManager>();
        _board.SendExpirience += IncreaseCurrentExpirienceAmount;
        _maxExpirienceAmount = (int)_xpManager.GetXPValue(EXPIRIENCE_PER_LEVEL, _currentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            IncreaseCurrentLevel();
    }

    private void OnDisable()
    {
        _board.SendExpirience -= IncreaseCurrentExpirienceAmount;
    }

    private void IncreaseCurrentExpirienceAmount(float value)
    {
        _currentExpirienceAmount += value;
        ExpirienceValueChanged?.Invoke(value);

        if (_currentExpirienceAmount >= _maxExpirienceAmount)
            IncreaseCurrentLevel();
    }

    public void IncreaseCurrentLevel()
    {
        LevelUpViewOpened?.Invoke(_bonusReward);

        _currentExpirienceAmount = _minExpirienceAmount;
        _currentLevel++;
        _maxExpirienceAmount = (int)_xpManager.GetXPValue(EXPIRIENCE_PER_LEVEL, _currentLevel);

        LevelValueChanged?.Invoke(_currentLevel, _maxExpirienceAmount);
    }
}
