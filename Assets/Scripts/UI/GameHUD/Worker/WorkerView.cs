using UnityEngine;

public class WorkerView : MonoBehaviour
{
    public TileType WorkerType => _workerType;

    [SerializeField]
    private TileType _workerType;
}
