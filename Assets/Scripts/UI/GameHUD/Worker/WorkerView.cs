using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerView : MonoBehaviour
{
    public TileType WorkerType => _workerType;

    [SerializeField]
    private TileType _workerType;
}
