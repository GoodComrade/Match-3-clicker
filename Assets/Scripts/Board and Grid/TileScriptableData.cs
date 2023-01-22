using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base tile Model", menuName = "Clicker Data Models/New tile data model")]
public class TileScriptableData : ScriptableObject
{
    [SerializeField] private Sprite _tile;
    [SerializeField] private float _baseReward;
    [SerializeField] private TileType _tileType;

    public Sprite Tile => _tile;
    public float BaseReward => _baseReward;
    public TileType TileType => _tileType;
    
}
