using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base tile Model", menuName = "Clicker Data Models/New tile data model")]
public class TileScriptableData : ScriptableObject
{
    [SerializeField] private Sprite _tile;
    [SerializeField] private float _baseReward;
    [SerializeField] private float _baseExpirience;
    [SerializeField] private TileType _tileType;

    private float _reward;
    private float _expirience;

    public Sprite Tile => _tile;
    public float BaseReward => _baseReward;
    public float Expirience => _expirience;
    public TileType TileType => _tileType;
    public float Reward => _reward;
    
    public void SetRewards(float value)
    {
        _reward += _baseReward * value;
        _expirience += _baseExpirience * value;
    }
    
    public void SetBaseReward()
    {
        _reward = _baseReward;
        _expirience = _baseExpirience;
    }
}
